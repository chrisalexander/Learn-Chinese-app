using LanguageModel;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LangDB
{
    [Export(typeof(IDatabaseParsingService))]
    public class DatabaseParsingService : IDatabaseParsingService
    {
        /// <summary>
        /// Parse a single line of the database file and return an entry if possible.
        /// </summary>
        /// <param name="line">The line to parse.</param>
        /// <param name="regex">The configured regex to use.</param>
        /// <returns>The language entry.</returns>
        public async Task<LanguageEntry> ParseLineAsync(string line, Regex regex)
        {
            return await Task.Run(() =>
            {
                // Disregard comment lines
                if (line.StartsWith("#"))
                {
                    return null;
                }

                var groups = regex.Match(line).Groups;

                var english = new List<string>();

                for (var i = 0; i < groups["english"].Captures.Count; i++)
                {
                    english.Add(groups["english"].Captures[i].Value);
                }

                LanguageEntry entry = null;

                try {
                    entry = new LanguageEntry(groups["traditional"].Value, groups["simplified"].Value, groups["pinyin"].Value, english);
                }
                catch (Exception) {}

                return entry;
            });
        }
    }
}
