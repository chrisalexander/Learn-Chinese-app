using LangDB;
using LongRunningProcess;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Composition;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;

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
        /// The overall progress of the operation.
        /// </summary>
        private double overallProgress;

        /// <summary>
        /// The overall status of the operation.
        /// </summary>
        private IEnumerable<string> overallStatus;

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
        /// The overall progress of the operation.
        /// </summary>
        public double OverallProgress
        {
            get
            {
                return this.overallProgress;
            }
            private set
            {
                this.SetProperty(ref this.overallProgress, value);
            }
        }

        /// <summary>
        /// The overall status of the operation.
        /// </summary>
        public IEnumerable<string> OverallStatus
        {
            get
            {
                return this.overallStatus;
            }
            private set
            {
                this.SetProperty(ref this.overallStatus, value);
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
                var messageDialog = new MessageDialog("There is already a language database in this folder, would you like to merge with it?", "Existing language database found");
                
                messageDialog.Commands.Add(new UICommand("Merge with existing file", new UICommandInvokedHandler((IUICommand cmd) =>
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

            IDatabaseAcquisitionResult result = null;
            Exception error = null;

            try
            {
                result = await this.dbService.AcquireAndParseArchiveAsync(new Uri(this.uri), this.targetFile, new Regex(this.regex), this.CreateProcess());
            }
            catch (Exception e)
            {
                error = e;
            }

            this.Enabled = true;

            if (result != null)
            {
                // Show the notification
                var toastTemplate = ToastTemplateType.ToastText01;
                var toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
                var toastTextElements = toastXml.GetElementsByTagName("text");
                toastTextElements[0].AppendChild(toastXml.CreateTextNode("Language database acquisition completed"));
                var toast = new ToastNotification(toastXml);
                ToastNotificationManager.CreateToastNotifier().Show(toast);

                // Show the info popup
                var builder = new StringBuilder();

                builder.AppendFormat("{0} entries in the database (previously: {1})", result.NewTotal, result.OldTotal); builder.AppendLine();
                builder.AppendFormat("{0} items added", result.Added); builder.AppendLine();
                builder.AppendFormat("{0} items modified", result.Modified); builder.AppendLine();
                builder.AppendFormat("{0} items removed", result.Removed); builder.AppendLine();
                builder.AppendFormat("{0} items not modified", result.Unmodified); builder.AppendLine();

                var messageDialog = new MessageDialog(builder.ToString(), "Acquisition completed");

                messageDialog.Commands.Add(new UICommand("OK"));

                messageDialog.DefaultCommandIndex = 0;
                messageDialog.CancelCommandIndex = 0;
                await messageDialog.ShowAsync();
                return;
            }

            if (error != null)
            {
                var messageDialog = new MessageDialog(error.Message, "Exception performing acquisition");

                messageDialog.Commands.Add(new UICommand("Done"));

                messageDialog.DefaultCommandIndex = 0;
                messageDialog.CancelCommandIndex = 0;
                await messageDialog.ShowAsync();
            }
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

            this.Process = this.processFactory.Create("Sync database");
            this.Process.PropertyChanged += this.ProcessChanged;

            return this.Process;
        }

        /// <summary>
        /// Handler for when the process changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The event arguments.</param>
        private void ProcessChanged(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case "OverallProgress":
                    this.OverallProgress = this.Process.OverallProgress;
                    break;
                case "OverallStatus":
                    this.OverallStatus = this.Process.OverallStatus;
                    break;
            }
        }
    }
}
