using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageModel;
using LongRunningProcess;

namespace LangDB.Windows
{
    /// <summary>
    /// Interface for services which merge new data in to databases.
    /// </summary>
    public interface IDatabaseMergeService
    {
        /// <summary>
        /// Merge the new entries on to the existing database, reporting on what was changed.
        /// </summary>
        /// <param name="database">The database to merge with.</param>
        /// <param name="newEntries">The new (and complete) list of entries to merge on.</param>
        /// <param name="process">The process.</param>
        /// <returns>Statistics regarding the merge.</returns>
        Task<IDatabaseAcquisitionResult> Merge(ILanguageDatabase database, IList<LanguageEntry> newEntries, IProcess process);
    }
}
