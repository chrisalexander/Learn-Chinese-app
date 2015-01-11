using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageModel
{
    /// <summary>
    /// Represents a Chinese character, in its written and pronounced forms.
    /// </summary>
    public class ChineseCharacter
    {
        /// <summary>
        /// Constructs the character from raw data.
        /// </summary>
        /// <param name="traditional">The traditional written form.</param>
        /// <param name="simplified">The simplified written form.</param>
        /// <param name="pinyin">The pinyin string.</param>
        public ChineseCharacter(char traditional, char simplified, string pinyin)
        {
            this.Traditional = traditional;
            this.Simplified = simplified;
            this.Mandarin = new PinyinSyllable(pinyin);
        }

        /// <summary>
        /// The traditional written form of the character.
        /// </summary>
        public char Traditional { get; private set; }

        /// <summary>
        /// The simplified written form of the character.
        /// </summary>
        public char Simplified { get; private set; }

        /// <summary>
        /// The mandarin pronounciation of the character, in Pinyin form.
        /// </summary>
        public PinyinSyllable Mandarin { get; private set; }
    }
}
