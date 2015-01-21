using LangDB;
using LongRunningProcess;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.ComponentModel;
using System.Composition;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;

namespace DBManager.ViewModels
{
    /// <summary>
    /// View model for the main page.
    /// </summary>
    [Export(typeof(MainPageViewModel))]
    public class MainPageViewModel : ViewModel
    {
        /// <summary>
        /// The database language service.
        /// </summary>
        private readonly ILanguageDatabaseService dbService;

        /// <summary>
        /// The process factory.
        /// </summary>
        private readonly IProcessFactory processFactory;

        /// <summary>
        /// The URI to access the data from.
        /// </summary>
        private string uri = "http://www.mdbg.net/chindict/export/cedict/cedict_1_0_ts_utf-8_mdbg.zip";

        /// <summary>
        /// The target file to store the database in.
        /// </summary>
        private StorageFile targetFile;

        /// <summary>
        /// The regex to use to separate data in a line of the file.
        /// </summary>
        private string regex = @"^(?<traditional>[^ ]+?) (?<simplified>[^ ]+?) \[(?<pinyin>[^\/]+?)\] \/((?<english>[^\/]+?)\/)*$";

        /// <summary>
        /// Whether the process is currently executing.
        /// </summary>
        private bool executing = false;

        /// <summary>
        /// MEF importing constructor.
        /// </summary>
        /// <param name="dbService">The database service.</param>
        /// <param name="processFactory">The process factory.</param>
        [ImportingConstructor]
        public MainPageViewModel(ILanguageDatabaseService dbService, IProcessFactory processFactory)
        {
            this.dbService = dbService;
            this.processFactory = processFactory;

            this.PickDatabaseFileCommand = DelegateCommand.FromAsyncHandler(this.PickDatabaseFileAsync, this.CanExecuteAsync);
            this.PickDatabaseFolderCommand = DelegateCommand.FromAsyncHandler(this.PickDatabaseFolderAsync, this.CanExecuteAsync);
            this.EngageCommand = DelegateCommand.FromAsyncHandler(this.EngageAsync, this.CanExecuteAsync);
            this.CancelCommand = DelegateCommand.FromAsyncHandler(this.CancelAsync);
        }

        /// <summary>
        /// The URI to the data file.
        /// </summary>
        public string Uri
        {
            get
            {
                return this.uri;
            }
            set
            {
                this.SetProperty(ref this.uri, value);
            }
        }

        /// <summary>
        /// The path to display for where the database will be saved.
        /// </summary>
        public string StoragePath
        {
            get
            {
                return this.targetFile == null ? string.Empty : this.targetFile.Path;
            }
        }

        /// <summary>
        /// The regex to parse the data with.
        /// </summary>
        public string Regex
        {
            get
            {
                return this.regex;
            }
            set
            {
                this.SetProperty(ref this.regex, value);
            }
        }

        /// <summary>
        /// Whether the UI should be enabled.
        /// </summary>
        public bool Enabled
        {
            get
            {
                return !this.executing;
            }
            private set
            {
                this.SetProperty(ref this.executing, !value);
            }
        }

        /// <summary>
        /// Whether the process is running.
        /// </summary>
        public bool Running
        {
            get
            {
                return this.Process != null && !this.Process.Completed;
            }
        }

        /// <summary>
        /// The process for ongoing operations.
        /// </summary>
        public IProcess Process { get; private set; }

        /// <summary>
        /// Command for picking a file to be the database.
        /// </summary>
        public ICommand PickDatabaseFileCommand { get; private set; }

        /// <summary>
        /// Command for picking a folder to place the database within.
        /// </summary>
        public ICommand PickDatabaseFolderCommand { get; private set; }

        /// <summary>
        /// Command to execute the acquisition.
        /// </summary>
        public ICommand EngageCommand { get; private set; }

        /// <summary>
        /// Command to cancel the acquisition.
        /// </summary>
        public ICommand CancelCommand { get; private set; }

        /// <summary>
        /// Execute the pick database file command.
        /// </summary>
        /// <returns>When complete.</returns>
        private async Task PickDatabaseFileAsync()
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".langdb");

            var file = await picker.PickSingleFileAsync();

            if (file == null)
            {
                return;
            }

            this.targetFile = file;
            this.OnPropertyChanged("StoragePath");
        }

        /// <summary>
        /// Execute the pick database folder command.
        /// </summary>
        /// <returns>When complete.</returns>
        private async Task PickDatabaseFolderAsync()
        {
            var picker = new FolderPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add("*");

            var folder = await picker.PickSingleFolderAsync();

            if (folder == null)
            {
                return;
            }

            StorageFile file = null;

            var existingFile = await folder.TryGetItemAsync("Chinese.langdb");
            if (existingFile != null)
            {
                // There is already a file with the expected name in the folder, does the user want to use that?
                var messageDialog = new MessageDialog("There is already a language database in this folder, would you like to use it?", "Existing language database found");
                
                messageDialog.Commands.Add(new UICommand("Use existing file", new UICommandInvokedHandler((IUICommand cmd) =>
                {
                    file = existingFile as StorageFile;
                })));
                
                messageDialog.Commands.Add(new UICommand("Pick again", new UICommandInvokedHandler((IUICommand cmd) =>
                {
                    file = null;
                })));

                messageDialog.DefaultCommandIndex = 0;
                messageDialog.CancelCommandIndex = 1;
                await messageDialog.ShowAsync();
            }
            else
            {
                file = await folder.CreateFileAsync("Chinese.langdb", CreationCollisionOption.OpenIfExists);
            }

            // If the user cancelled picking a file then don't set it.
            if (file == null)
            {
                return;
            }

            this.targetFile = file;
            this.OnPropertyChanged("StoragePath");
        }

        /// <summary>
        /// Execute the engage command.
        /// </summary>
        /// <returns>When complete.</returns>
        private async Task EngageAsync()
        {
            // Validate they have picked a file.
            if (this.targetFile == null)
            {
                var messageDialog = new MessageDialog("You have not picked a file; please pick one to continue.", "Please pick a file");

                messageDialog.Commands.Add(new UICommand("OK"));

                messageDialog.DefaultCommandIndex = 0;
                messageDialog.CancelCommandIndex = 0;
                await messageDialog.ShowAsync();
                return;
            }

            this.Enabled = false;

            try
            {
                await this.dbService.AcquireAndParseArchiveAsync(new Uri(this.uri), this.targetFile, new Regex(this.regex), this.CreateProcess());
            }
            catch (Exception) { }

            this.Enabled = true;
        }

        /// <summary>
        /// Execute the cancel command.
        /// </summary>
        /// <returns>When complete.</returns>
        private async Task CancelAsync()
        {
            // Can't cancel if not running.
            if (this.Process == null || this.Process.Completed)
            {
                return;
            }

            await this.Process.CancelAsync();
        }

        /// <summary>
        /// Whether commands can execute is based on whether we are currently executing.
        /// </summary>
        /// <returns>Whether the command can be executed.</returns>
        private bool CanExecuteAsync()
        {
            return !this.executing;
        }

        /// <summary>
        /// Create a process to be used for the exceution.
        /// </summary>
        /// <returns>The process.</returns>
        private IProcess CreateProcess()
        {
            if (this.Process != null)
            {
                this.Process.PropertyChanged -= this.ProcessChanged;
            }

            this.Process = this.processFactory.Create("Create database");
            this.Process.PropertyChanged += this.ProcessChanged;

            this.OnPropertyChanged("Process");
            this.OnPropertyChanged("Running");

            return this.Process;
        }

        /// <summary>
        /// Handler for when the process changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The event arguments.</param>
        private void ProcessChanged(object sender, PropertyChangedEventArgs args)
        {
            this.OnPropertyChanged("Process");
            this.OnPropertyChanged("Running");
        }
    }
}
