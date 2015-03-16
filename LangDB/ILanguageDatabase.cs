﻿using System;
using System.Collections.Generic;
using DBUtils;
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
        IDictionary<LanguageEntryId, LanguageEntry> Entries { get; }

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
