using LangDB.Model;
using LongRunningProcess;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;

namespace LangDB.Services
{
    /// <summary>
    /// Interface for services which provide high-level operations on databases.
    /// </summary>
    public interface ILanguageDatabaseService
    {
        /// <summary>
        /// Acquire an archive from a URI, and merge with an existing file or create a new one if it exists.
        /// </summary>
        /// <param name="uri">The URI of the archive.</param>
        /// <param name="file">The existing archive file to merge with.</param>
        /// <param name="regex">The regex to apply to extract data from each row of the archive.</param>
        /// <param name="process">The process.</param>
        /// <returns>When complete.</returns>
        Task<IDatabaseAcquisitionResult> AcquireAndParseArchiveAsync(Uri uri, IStorageFile file, Regex regex, IProcess process);
    }
}
