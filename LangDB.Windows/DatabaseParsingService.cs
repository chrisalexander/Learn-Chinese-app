using System;
using System.Collections.Generic;
using System.Composition;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LanguageModel;
using LongRunningProcess;

namespace LangDB.Windows
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

            await process.RunInBackground((progress, token) =>
            {
                foreach (var line in lines)
                {
                    var entry = this.ParseLine(line, regex);
                    if (entry != null)
                    {
                        entries.Add(entry);
                    }
                    progress.Report(stepSize);
                    token.ThrowIfCancellationRequested();
                }
            }, true);

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
            return await Task.Run(() => this.ParseLine(line, regex));
        }

        /// <summary>
        /// Helper which synchronously parses a line.
        /// </summary>
        /// <param name="line">The line to parse.</param>
        /// <param name="regex">The configuration regex to use.</param>
        /// <returns>The language entry.</returns>
        private LanguageEntry ParseLine(string line, Regex regex)
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

            try
            {
                entry = new LanguageEntry(groups["traditional"].Value, groups["simplified"].Value, groups["pinyin"].Value, english);
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception) { }

            return entry;
        }
    }
}
