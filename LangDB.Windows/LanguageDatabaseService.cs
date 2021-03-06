﻿using System;
using System.Collections.Generic;
using System.Composition;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using DBUtils.Windows;
using LongRunningProcess;

namespace LangDB.Windows
{
    /// <summary>
    /// Database service for language operations.
    /// </summary>
    [Export(typeof(ILanguageDatabaseService))]
    public class LanguageDatabaseService : AbstractFileServiceWrapper<LanguageDatabase, ILanguageDatabase>, ILanguageDatabaseService
    {
        /// <summary>
        /// The archive acquisition service.
        /// </summary>
        private readonly IArchiveAcquisitionService archiveAcquisitionService;

        /// <summary>
        /// The parsing service.
        /// </summary>
        private readonly IDatabaseParsingService parsingService;

        /// <summary>
        /// The database file service.
        /// </summary>
        private readonly IDatabaseFileService<ILanguageDatabase> fileService;

        /// <summary>
        /// The database merge service.
        /// </summary>
        private readonly IDatabaseMergeService mergeService;

        /// <summary>
        /// The supported file extensions of language databases.
        /// </summary>
        private readonly IEnumerable<string> extensions = new[] { ".langdb" };

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="archiveAcquisitionService">The archive acquisition service.</param>
        /// <param name="parsingService">The parsing service.</param>
        /// <param name="fileService">The file service.</param>
        /// <param name="mergeService">The database merge service.</param>
        [ImportingConstructor]
        public LanguageDatabaseService(
            IArchiveAcquisitionService archiveAcquisitionService,
            IDatabaseParsingService parsingService,
            IDatabaseFileService<ILanguageDatabase> fileService,
            IDatabaseMergeService mergeService)
        {
            this.archiveAcquisitionService = archiveAcquisitionService;
            this.parsingService = parsingService;
            this.fileService = fileService;
            this.mergeService = mergeService;
        }

        /// <summary>
        /// Provide the file service to the abstract wrapper.
        /// </summary>
        protected override IDatabaseFileService<ILanguageDatabase> FileService
        {
            get
            {
                return this.fileService;
            }
        }

        /// <summary>
        /// Provide the default picker location to the abstract wrapper.
        /// </summary>
        protected override PickerLocationId DefaultPickerLocation
        {
            get
            {
                return PickerLocationId.DocumentsLibrary;
            }
        }

        /// <summary>
        /// Provide the supported extensions to the abstract wrapper.
        /// </summary>
        protected override IEnumerable<string> Extensions
        {
            get
            {
                return this.extensions;
            }
        }

        /// <summary>
        /// Acquire an archive from a URI, and merge with an existing file or create a new one if it exists.
        /// </summary>
        /// <param name="uri">The URI of the archive.</param>
        /// <param name="file">The existing archive file to merge with.</param>
        /// <param name="regex">The regex to apply to extract data from each row of the archive.</param>
        /// <param name="process">The process.</param>
        /// <returns>When complete.</returns>
        public async Task<IDatabaseAcquisitionResult> AcquireAndParseArchiveAsync(Uri uri, IStorageFile file, Regex regex, IProcess process)
        {
            ILanguageDatabase database = null;

            // If the language database fails to load, then we simply create an empty one.
            try
            {
                database = await this.fileService.LoadAsync<LanguageDatabase>(file, process.Step("Loading database", 10));
            }
            catch (TaskCanceledException)
            {
                // Ensure cancellations are correctly rethrown.
                throw;
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception)
            {
            }

            if (database == null)
            {
                database = new LanguageDatabase();
            }

            database.Source = uri.ToString();

            var lines = await this.archiveAcquisitionService.GetLinesAsync(uri, process.Step("Acquiring database", 20));

            var entries = await this.parsingService.ParseLinesAsync(lines, regex, process.Step("Parsing database entries", 30));

            var result = await this.mergeService.Merge(database, entries, process.Step("Merging databases", 30));

            await this.SaveAsync(database, file, process.Step("Saving database", 10));

            process.Completed = true;

            return result;
        }

        /// <summary>
        /// Provide metadata regarding the database prior to save.
        /// </summary>
        /// <param name="database">The database.</param>
        /// <param name="process">The process.</param>
        /// <returns>When complete.</returns>
        protected override Task<bool> PreSaveStep(ILanguageDatabase database, IProcess process)
        {
            database.Updated = DateTime.Now;

            process.Completed = true;

            return Task.FromResult(true);
        }
    }
}
