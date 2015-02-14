using Microsoft.Practices.Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Composition;

namespace LevelEditor.ViewModels
{
    /// <summary>
    /// View model for the main page.
    /// </summary>
    [Export(typeof(MainPageViewModel))]
    public class MainPageViewModel : ViewModel
    {
        /// <summary>
        /// The databases in the view.
        /// </summary>
        private ObservableCollection<CourseDatabaseViewModel> databases;

        /// <summary>
        /// The currently selected database.
        /// </summary>
        private CourseDatabaseViewModel selectedDatabase;

        /// <summary>
        /// MEF importing constructor.
        /// </summary>
        [ImportingConstructor]
        public MainPageViewModel()
        {
            this.databases = new ObservableCollection<CourseDatabaseViewModel>();
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
    }
}
