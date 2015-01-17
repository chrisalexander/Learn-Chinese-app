using LangDB;
using Microsoft.Practices.Prism.Mvvm;
using System.Composition;

namespace DBManager.ViewModels
{
    /// <summary>
    /// View model for the main page.
    /// </summary>
    [Export(typeof(MainPageViewModel))]
    public class MainPageViewModel : ViewModel
    {
        /// <summary>
        /// The database file service.
        /// </summary>
        private readonly ILanguageDatabaseFileService dbFileService;

        /// <summary>
        /// MEF importing constructor.
        /// </summary>
        /// <param name="dbFileService">The database file service.</param>
        [ImportingConstructor]
        public MainPageViewModel(ILanguageDatabaseFileService dbFileService)
        {
            this.dbFileService = dbFileService;
        }

        public string Uri
        {
            get
            {
                return "http://chris-alexander.co.uk";
            }
        }
    }
}
