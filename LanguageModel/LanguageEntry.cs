using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageModel
{
    /// <summary>
    /// A language entry is a Chinese word and its English equivalents.
    /// </summary>
    public class LanguageEntry
    {
        /// <summary>
        /// Creates a new entry with all of the requirements.
        /// </summary>
        /// <param name="traditional">The Chinese traditional written form.</param>
        /// <param name="simplified">The Chinese simplified written form.</param>
        /// <param name="pinyin">The Mandarin Chinese pronounciation in Pinyin format.</param>
        /// <param name="english">A list of English translations and notes.</param>
        public LanguageEntry(string traditional, string simplified, string pinyin, IEnumerable<string> english)
        {
            this.Chinese = new ChineseWord(traditional, simplified, pinyin);
            this.English = english;
        }

        /// <summary>
        /// ID of the entry, which is formed of the traditional characters.
        /// </summary>
        public string Id
        {
            get
            {
                var builder = new StringBuilder();
                
                foreach (var character in this.Chinese.Characters)
                {
                    builder.Append(character.Traditional);
                }

                return builder.ToString();
            }
        }

        /// <summary>
        /// The Chinese word.
        /// </summary>
        public ChineseWord Chinese { get; private set; }

        /// <summary>
        /// The English translations and notes of the Chinese word.
        /// </summary>
        public IEnumerable<string> English { get; private set; }
    }
}
