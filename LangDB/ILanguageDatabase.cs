using LanguageModel;
using System;
using System.Collections.Generic;

namespace LangDB
{
    /// <summary>
    /// Interface defining a language database.
    /// </summary>
    public interface ILanguageDatabase
    {
        /// <summary>
        /// The entries in the database.
        /// </summary>
        IDictionary<string, LanguageEntry> Entries { get; }

        /// <summary>
        /// The path to the database file.
        /// </summary>
        string Path { get; set; }
    }
}
