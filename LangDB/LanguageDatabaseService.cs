using System;
using System.Composition;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;

namespace LangDB
{
    /// <summary>
    /// Database service for language operations.
    /// </summary>
    [Export(typeof(ILanguageDatabaseService))]
    public class LanguageDatabaseService : ILanguageDatabaseService
    {
        /// <summary>
        /// The archive acquisition service.
        /// </summary>
        private IArchiveAcquisitionService archiveAcquisitionService;

        /// <summary>
        /// The parsing service.
        /// </summary>
        private IDatabaseParsingService parsingService;

        /// <summary>
        /// The language database file service.
        /// </summary>
        private ILanguageDatabaseFileService fileService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="archiveAcquisitionService">The archive acquisition service.</param>
        /// <param name="parsingService">The parsing service.</param>
        /// <param name="fileService">The file service.</param>
        [ImportingConstructor]
        public LanguageDatabaseService(
            IArchiveAcquisitionService archiveAcquisitionService,
            IDatabaseParsingService parsingService,
            ILanguageDatabaseFileService fileService)
        {
            this.archiveAcquisitionService = archiveAcquisitionService;
            this.parsingService = parsingService;
            this.fileService = fileService;
        }

        /// <summary>
        /// Acquire an archive from a URI, and merge with an existing file or create a new one if it exists.
        /// </summary>
        /// <param name="uri">The URI of the archive.</param>
        /// <param name="file">The existing archive file to merge with.</param>
        /// <param name="regex">The regex to apply to extract data from each row of the archive.</param>
        /// <returns>When complete.</returns>
        public async Task AcquireAndParseArchiveAsync(Uri uri, StorageFile file, Regex regex)
        {
            var database = new LanguageDatabase(file.Path);

            var lines = await this.archiveAcquisitionService.GetLinesAsync(uri);

            foreach (var line in lines)
            {
                var entry = await this.parsingService.ParseLineAsync(line, regex);
                if (entry != null)
                {
                    database.Entries.Add(entry);
                }
            }

            await this.fileService.SaveAsync(database, file);
        }
    }
}
