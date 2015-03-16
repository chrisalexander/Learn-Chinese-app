using System.Collections.Generic;
using System.Linq;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using CourseDB.Model;
using CourseDB.Services;
using LangDB.Model;
using LangDB.Services;
using LongRunningProcess;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Composition;
using System.Threading.Tasks;
using System.Windows.Input;

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
        private readonly ILanguageDatabaseService languageDatabaseService;

        /// <summary>
        /// The course database service.
        /// </summary>
        private readonly ICourseDatabaseService courseDatabaseService;

        /// <summary>
        /// The process factory.
        /// </summary>
        private readonly IProcessFactory processFactory;

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
        /// <param name="courseDatabaseService">The course database service.</param>
        /// <param name="processFactory">The process factory.</param>
        [ImportingConstructor]
        public MainPageViewModel(ILanguageDatabaseService languageDatabaseService, ICourseDatabaseService courseDatabaseService, IProcessFactory processFactory)
        {
            this.languageDatabaseService = languageDatabaseService;
            this.courseDatabaseService = courseDatabaseService;
            this.processFactory = processFactory;

            this.databases = new ObservableCollection<CourseDatabaseViewModel>();
            this.PickLanguageDatabaseCommand = DelegateCommand.FromAsyncHandler(this.PickLanguageDatabaseAsync, this.CanExecuteAsync);
            this.NewCourseDatabaseCommand = DelegateCommand.FromAsyncHandler(this.NewCourseDatabaseAsync, this.CanExecuteAsync);
            this.OpenCourseDatabaseCommand = DelegateCommand.FromAsyncHandler(this.OpenCourseDatabaseAsync, this.CanExecuteAsync);
            this.CloseCourseDatabaseCommand = new DelegateCommand(this.CloseCourseDatabase, this.CanExecuteAsync);
            this.DeleteCourseDatabaseCommand = DelegateCommand.FromAsyncHandler(this.DeleteCourseDatabaseAsync, this.CanExecuteAsync);
            this.SaveCommand = DelegateCommand.FromAsyncHandler(this.SaveAsync, this.CanExecuteAsync);

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
        /// New course database command.
        /// </summary>
        public ICommand NewCourseDatabaseCommand { get; private set; }

        /// <summary>
        /// Open course database command.
        /// </summary>
        public ICommand OpenCourseDatabaseCommand { get; private set; }

        /// <summary>
        /// Close course database command.
        /// </summary>
        public ICommand CloseCourseDatabaseCommand { get; private set; }

        /// <summary>
        /// Delete course database command.
        /// </summary>
        public ICommand DeleteCourseDatabaseCommand { get; private set; }

        /// <summary>
        /// Save all command.
        /// </summary>
        public ICommand SaveCommand { get; private set; }

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

        /// <summary>
        /// Execute new course database command.
        /// </summary>
        /// <returns>When complete.</returns>
        private async Task NewCourseDatabaseAsync()
        {
            this.Enabled = false;
            
            var database = new Course();
            database.Id = new CourseId();
            database.Name = database.Id.ToString();

            try
            {
                var file = await this.courseDatabaseService.CreateAsync(database.Id.RootId.ToString(), this.processFactory.Create("Create course database"));

                await this.courseDatabaseService.SaveAsync(database, file, this.processFactory.Create("Create blank database"));

                this.CompleteDatabaseLoad(database, file);
            }
            catch (Exception e)
            {
                this.ShowError("Create database", e.Message);
            }

            this.Enabled = true;
        }

        /// <summary>
        /// Execute open course database command.
        /// </summary>
        /// <returns>When complete.</returns>
        private async Task OpenCourseDatabaseAsync()
        {
            this.Enabled = false;

            try
            {
                var databaseFile = await this.courseDatabaseService.OpenWithoutParseAsync(this.processFactory.Create("Open course database"));

                if (databaseFile != null)
                {
                    var database = await this.courseDatabaseService.LoadAsync(databaseFile, this.processFactory.Create("Parse course database"));
                    this.CompleteDatabaseLoad(database, databaseFile);
                }
            }
            catch (Exception e)
            {
                this.ShowError("Open database", e.Message);
            }

            this.Enabled = true;
        }

        /// <summary>
        /// Execute close course database command.
        /// </summary>
        /// <returns>When complete.</returns>
        private void CloseCourseDatabase()
        {
            this.Enabled = false;

            this.RemoveCurrentDatabaseSelection();

            this.Enabled = true;
        }

        /// <summary>
        /// Execute delete course database command.
        /// </summary>
        /// <returns>When complete.</returns>
        private async Task DeleteCourseDatabaseAsync()
        {
            this.Enabled = false;

            var canDelete = await this.CanDeleteDatabaseAsync(this.SelectedDatabase.Name);

            if (canDelete)
            {
                var database = this.SelectedDatabase;
                this.RemoveCurrentDatabaseSelection();
                await this.courseDatabaseService.DeleteAsync(database.StorageFile, this.processFactory.Create("Delete database"));
            }

            this.Enabled = true;
        }

        /// <summary>
        /// Save the data.
        /// </summary>
        /// <returns>When complete</returns>
        private async Task SaveAsync()
        {
            this.Enabled = false;

            var process = this.processFactory.Create("Save all courses");
            var weight = 100.0 / this.Databases.Count;

            var tasks = new List<Task>();

            foreach (var course in this.Databases)
            {
                var innerCourse = course;
                var processStep = process.Step("Save course " + innerCourse.Name, weight);
                var courseSource = await process.RunInBackground((progress, token) => innerCourse.ToSource());
                var task = this.courseDatabaseService.SaveAsync(courseSource, innerCourse.StorageFile, processStep);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            this.Enabled = true;
        }

        /// <summary>
        /// Helper that takes a database and its file and add them to the UI.
        /// </summary>
        /// <param name="database">The parsed database.</param>
        /// <param name="file">The file the database came from.</param>
        private void CompleteDatabaseLoad(ICourse database, IStorageFile file)
        {
            var newDatabase = new CourseDatabaseViewModel(database, file);

            if (this.Databases.Any(db => db.Id.Equals(database.Id)))
            {
                this.ShowError("Add Database", "This database is already opened.");
                return;
            }

            this.Databases.Add(newDatabase);
            this.SelectedDatabase = newDatabase;
        }

        /// <summary>
        /// Remove the current database selection from the selection and the list of databases.
        /// </summary>
        private void RemoveCurrentDatabaseSelection()
        {
            this.Databases.Remove(this.SelectedDatabase);

            if (this.Databases.Count > 0)
            {
                this.SelectedDatabase = this.Databases[0];
            }
            else
            {
                this.SelectedDatabase = null;
            }
        }

        /// <summary>
        /// Check whether the user really wants to delete the database.
        /// </summary>
        /// <param name="databaseName">The name of the database to delete.</param>
        /// <returns>Whether the user wishes to delete the file.</returns>
        private async Task<bool> CanDeleteDatabaseAsync(string databaseName)
        {
            var canUse = false;

            var messageDialog = new MessageDialog("Would you like to delete the level database " + databaseName, "Are you sure?");

            messageDialog.Commands.Add(new UICommand("Delete file", cmd =>
            {
                canUse = true;
            }));

            messageDialog.Commands.Add(new UICommand("Cancel", cmd =>
            {
                canUse = false;
            }));

            messageDialog.DefaultCommandIndex = 1;
            messageDialog.CancelCommandIndex = 1;
            await messageDialog.ShowAsync();

            return canUse;
        }

        /// <summary>
        /// Show an error to the user.
        /// </summary>
        /// <param name="action">What was happening.</param>
        /// <param name="errorDescription">The error message.</param>
        /// <returns>When shown.</returns>
        private void ShowError(string action, string errorDescription)
        {
            var toastTemplate = ToastTemplateType.ToastText02;
            var toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
            var toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode("Error: " + action));
            toastTextElements[1].AppendChild(toastXml.CreateTextNode(errorDescription));
            var toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
