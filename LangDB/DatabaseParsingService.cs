using LanguageModel;
using LongRunningProcess;
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
        /// Parse a list of lines.
        /// </summary>
        /// <param name="lines">The lines to parse.</param>
        /// <param name="regex">The configured regex to use.</param>
        /// <param name="process">The process.</param>
        /// <returns>The language entries.</returns>
        public async Task<IList<LanguageEntry>> ParseLinesAsync(IList<string> lines, Regex regex, IProcess process)
        {
            process.DurationType = ProcessDurationType.Determinate;

            var stepSize = 100.0 / lines.Count;

            var entries = new List<LanguageEntry>();

            foreach (var line in lines)
            {
                var entry = await this.ParseLineAsync(line, regex);
                if (entry != null)
                {
                    entries.Add(entry);
                }
                process.Increment(stepSize);
            }

            process.Completed = true;

            return entries;
        }

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
