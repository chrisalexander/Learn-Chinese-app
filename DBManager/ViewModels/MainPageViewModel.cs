using LangDB;
using Microsoft.Practices.Prism.Mvvm;
using System.Composition;
using Windows.Storage;

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
        private string regex = "herpderp123";

        /// <summary>
        /// MEF importing constructor.
        /// </summary>
        /// <param name="dbFileService">The database file service.</param>
        [ImportingConstructor]
        public MainPageViewModel(ILanguageDatabaseFileService dbFileService)
        {
            this.dbFileService = dbFileService;
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
    }
}
