using LangDB.Model;
using LangDB.Services;
using LongRunningProcess;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Composition;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage.Pickers;

namespace LevelEditor.ViewModels
{
    /// <summary>
    /// View model for the main page.
    /// </summary>
    [Export(typeof(MainPageViewModel))]
    public class MainPageViewModel : ViewModel
    {
        /// <summary>
        /// The language database service.
        /// </summary>
        private ILanguageDatabaseService languageDatabaseService;

        /// <summary>
        /// The process factory.
        /// </summary>
        private IProcessFactory processFactory;

        /// <summary>
        /// The databases in the view.
        /// </summary>
        private ObservableCollection<CourseDatabaseViewModel> databases;

        /// <summary>
        /// The currently selected database.
        /// </summary>
        private CourseDatabaseViewModel selectedDatabase;

        /// <summary>
        /// The language database to use.
        /// </summary>
        private ILanguageDatabase languageDatabase;

        /// <summary>
        /// Whether the UI controls should be enabled.
        /// </summary>
        private bool enabled;

        /// <summary>
        /// MEF importing constructor.
        /// </summary>
        /// <param name="languageDatabaseService">The language database service.</param>
        [ImportingConstructor]
        public MainPageViewModel(ILanguageDatabaseService languageDatabaseService, IProcessFactory processFactory)
        {
            this.languageDatabaseService = languageDatabaseService;
            this.processFactory = processFactory;

            this.databases = new ObservableCollection<CourseDatabaseViewModel>();
            this.PickLanguageDatabaseCommand = DelegateCommand.FromAsyncHandler(this.PickLanguageDatabaseAsync, this.CanExecuteAsync);

            this.Enabled = true;
        }

        /// <summary>
        /// The databases in the view.
        /// </summary>
        public ObservableCollection<CourseDatabaseViewModel> Databases
        {
            get
            {
                return this.databases;
            }

            set
            {
                this.SetProperty(ref this.databases, value);
            }
        }

        /// <summary>
        /// The selected database.
        /// </summary>
        public CourseDatabaseViewModel SelectedDatabase
        {
            get
            {
                return this.selectedDatabase;
            }

            set
            {
                this.SetProperty(ref this.selectedDatabase, value);
            }
        }

        /// <summary>
        /// The language database to use.
        /// </summary>
        public ILanguageDatabase LanguageDatabase
        {
            get
            {
                return this.languageDatabase;
            }

            set
            {
                this.SetProperty(ref this.languageDatabase, value);
            }
        }

        /// <summary>
        /// Whether the UI should be enabled.
        /// </summary>
        public bool Enabled
        {
            get
            {
                return this.enabled;
            }

            set
            {
                this.SetProperty(ref this.enabled, value);
            }
        }

        /// <summary>
        /// Command for picking the language database.
        /// </summary>
        public ICommand PickLanguageDatabaseCommand { get; private set; }

        /// <summary>
        /// Whether commands can execute is based on whether we are currently executing.
        /// </summary>
        /// <returns>Whether the command can be executed.</returns>
        private bool CanExecuteAsync()
        {
            return this.enabled;
        }

        /// <summary>
        /// Execute the command to pick the language database.
        /// </summary>
        /// <returns>When complete.</returns>
        private async Task PickLanguageDatabaseAsync()
        {
            this.Enabled = false;
            this.LanguageDatabase = await this.languageDatabaseService.OpenAsync(this.processFactory.Create("Pick file"));
            this.Enabled = true;
        }
    }
}
