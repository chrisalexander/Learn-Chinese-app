using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LanguageModel;
using SimpleInMemorySearch;

namespace LangDB
{
    /// <summary>
    /// The language search service.
    /// </summary>
    [Export(typeof(ILanguageSearchService))]
    public class LanguageSearchService : AbstractSearchService<ILanguageEntry, LanguageEntryId>, ILanguageSearchService
    {
        /// <summary>
        /// Regular expression to leave only characters, numbers and whitespace in a string.
        /// </summary>
        private static readonly Regex EnglishTokeniserRegex = new Regex(@"[^a-zA-Z\d\s:]");

        /// <summary>
        /// Index an entire language database.
        /// </summary>
        /// <param name="database">The language database.</param>
        /// <returns>When complete.</returns>
        public async Task Index(ILanguageDatabase database)
        {
            await Task.WhenAll(database.Entries.Select(this.Index));
        }

        /// <summary>
        /// Returns the keywords for the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The keywords.</returns>
        protected override IEnumerable<ScoredItemKeyword> Keywords(ILanguageEntry item)
        {
            // Tokenise the English phrases and yield those
            foreach (var english in item.English)
            {
                var isolated = EnglishTokeniserRegex.Replace(english, string.Empty);
                var tokens = isolated.Split(' ');
                foreach (var token in tokens)
                {
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        yield return new ScoredItemKeyword(token, 0);
                    }
                }
            }

            // Return the simplified and traditional forms of Chinese
            yield return new ScoredItemKeyword(string.Join(string.Empty, item.Chinese.Characters.Select(c => c.Traditional)), 1);
            yield return new ScoredItemKeyword(string.Join(string.Empty, item.Chinese.Characters.Select(c => c.Simplified)), 1);

            // Return each of the pinyin syllables, in reverse priority order
            var numberOfCharacters = item.Chinese.Characters.Count();

            foreach (var character in item.Chinese.Characters)
            {
                yield return new ScoredItemKeyword(character.Mandarin.ToString(), numberOfCharacters);
                numberOfCharacters--;
            }
        }
    }
}
