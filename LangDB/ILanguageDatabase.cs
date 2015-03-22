using System;
using DBUtils;
using KeyedItemCollection;
using LanguageModel;

namespace LangDB
{
    /// <summary>
    /// Interface defining a language database.
    /// </summary>
    public interface ILanguageDatabase : IDatabase
    {
        /// <summary>
        /// The entries in the database.
        /// </summary>
        IKeyedItemCollection<LanguageEntry, LanguageEntryId> Entries { get; }

        /// <summary>
        /// The source of the data.
        /// </summary>
        string Source { get; set; }

        /// <summary>
        /// The time the file was last updated.
        /// </summary>
        DateTime Updated { get; set; }
    }
}
