using DBUtils.Model;
using LanguageModel;
using System;
using System.Collections.Generic;

namespace LangDB.Model
{
    /// <summary>
    /// Interface defining a language database.
    /// </summary>
    public interface ILanguageDatabase : IDatabase
    {
        /// <summary>
        /// The entries in the database.
        /// </summary>
        IDictionary<string, LanguageEntry> Entries { get; }

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
