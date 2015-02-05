using LanguageModel;
using LongRunningProcess;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;

namespace LangDB
{
    /// <summary>
    /// Basic implementation of database merging.
    /// </summary>
    [Export(typeof(IDatabaseMergeService))]
    public class DatabaseMergeService : IDatabaseMergeService
    {
        /// <summary>
        /// Merge the new entries on to the existing database, reporting on what was changed.
        /// </summary>
        /// <param name="database">The database to merge with.</param>
        /// <param name="newEntries">The new (and complete) list of entries to merge on.</param>
        /// <param name="process">The process.</param>
        /// <returns>Statistics regarding the merge.</returns>
        public async Task<IDatabaseAcquisitionResult> Merge(ILanguageDatabase database, IList<LanguageEntry> newEntries, IProcess process)
        {
            return await process.RunInBackground((progress, token) => this.ExecuteMerge(database, newEntries, progress, token), true);
        }

        /// <summary>
        /// Executes the merge.
        /// </summary>
        /// <param name="database">The database to merge with.</param>
        /// <param name="newEntries">The new (and complete) list of entries to merge on.</param>
        /// <param name="progress">The progress object.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>Statistics regarding the merge.</returns>
        private IDatabaseAcquisitionResult ExecuteMerge(ILanguageDatabase database, IList<LanguageEntry> newEntries, IProgress<double> progress, CancellationToken token)
        {
            var foundEntryIds = new HashSet<string>();

            var result = new DatabaseAcquisitionResult();
            result.NewTotal = newEntries.Count;
            result.OldTotal = database.Entries.Count;

            var increment = 100.0 / (newEntries.Count + database.Entries.Count);

            var entriesToAdd = new List<LanguageEntry>();

            foreach (var entry in newEntries)
            {
                foundEntryIds.Add(entry.Id);
                progress.Report(increment);
                token.ThrowIfCancellationRequested();

                LanguageEntry existingEntry;

                if (!database.Entries.TryGetValue(entry.Id, out existingEntry))
                {
                    // If we weren't able to get it, then we can add and continue
                    entriesToAdd.Add(entry);
                    result.Added++;
                    continue;
                }

                if (existingEntry.Hash != entry.Hash)
                {
                    entriesToAdd.Add(entry);
                    result.Modified++;
                    continue;
                }

                result.Unmodified++;
            }

            var entriesToRemove = new List<string>();

            // Work out which old entries need to be removed then remove them
            foreach (var oldEntry in database.Entries)
            {
                // If it is not found in the existing set then flag it for removal
                if (!foundEntryIds.Contains(oldEntry.Key))
                {
                    entriesToRemove.Add(oldEntry.Key);
                }

                progress.Report(increment);

                token.ThrowIfCancellationRequested();
            }

            foreach (var entryId in entriesToRemove)
            {
                database.Entries.Remove(entryId);
                result.Removed++;
            }

            // Apply the changes we want to make
            foreach (var entry in entriesToAdd)
            {
                database.Entries[entry.Id] = entry;
            }

            return result;
        }
    }
}
