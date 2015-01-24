using LanguageModel;
using LongRunningProcess;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var foundEntryIds = new HashSet<string>();

            var result = new DatabaseAcquisitionResult();
            result.NewTotal = newEntries.Count;
            result.OldTotal = database.Entries.Count;

            var increment = 100.0 / (newEntries.Count + database.Entries.Count);

            var token = process.CancellationToken;

            var entriesToAdd = new List<LanguageEntry>();

            foreach (var entry in newEntries)
            {
                foundEntryIds.Add(entry.Id);
                process.Increment(increment);
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

                process.Increment(increment);

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

            process.Complete();

            return result;
        }
    }
}
