using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LanguageModel;
using LongRunningProcess;

namespace LangDB.Windows
{
    /// <summary>
    /// Interface for services which parse raw database files in to language model components.
    /// </summary>
    public interface IDatabaseParsingService
    {
        /// <summary>
        /// Parse a list of lines.
        /// </summary>
        /// <param name="lines">The lines to parse.</param>
        /// <param name="regex">The configured regex to use.</param>
        /// <param name="process">The process.</param>
        /// <returns>The language entries.</returns>
        Task<IList<LanguageEntry>> ParseLinesAsync(IList<string> lines, Regex regex, IProcess process);

        /// <summary>
        /// Parse a single line of the database file and return an entry if possible.
        /// </summary>
        /// <param name="line">The line to parse.</param>
        /// <param name="regex">The configured regex to use.</param>
        /// <returns>The language entry.</returns>
        Task<LanguageEntry> ParseLineAsync(string line, Regex regex);
    }
}
