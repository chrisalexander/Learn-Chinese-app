using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LongRunningProcess;

namespace LangDB.Windows
{
    /// <summary>
    /// Interface for services to acquire archives and read them.
    /// </summary>
    public interface IArchiveAcquisitionService
    {
        /// <summary>
        /// Acquire a URI and return the lines within the file.
        /// </summary>
        /// <param name="uri">The URI of the archive to retrieve.</param>
        /// <param name="process">The process.</param>
        /// <returns>Each of the lines in the file.</returns>
        Task<IList<string>> GetLinesAsync(Uri uri, IProcess process);
    }
}
