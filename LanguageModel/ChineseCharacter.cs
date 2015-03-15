using LanguageModel.Annotations;
using Newtonsoft.Json;

namespace LanguageModel
{
    /// <summary>
    /// Represents a Chinese character, in its written and pronounced forms.
    /// </summary>
    public class ChineseCharacter : IChineseCharacter
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
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="traditional">Traditional character.</param>
        /// <param name="simplified">Simplified character.</param>
        /// <param name="mandarin">Mandarin Pinyin.</param>
        [JsonConstructor, UsedImplicitly]
        private ChineseCharacter(char traditional, char simplified, PinyinSyllable mandarin)
        {
            this.Traditional = traditional;
            this.Simplified = simplified;
            this.Mandarin = mandarin;
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
        public IPinyinSyllable Mandarin { get; private set; }
    }
}
