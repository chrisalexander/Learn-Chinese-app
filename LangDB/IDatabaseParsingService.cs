﻿using LanguageModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LangDB
{
    /// <summary>
    /// Interface for services which parse raw database files in to language model components.
    /// </summary>
    public interface IDatabaseParsingService
    {
        /// <summary>
        /// Parse a single line of the database file and return an entry if possible.
        /// </summary>
        /// <param name="line">The line to parse.</param>
        /// <param name="regex">The configured regex to use.</param>
        /// <returns>The language entry.</returns>
        Task<LanguageEntry> ParseLineAsync(string line, Regex regex);
    }
}
