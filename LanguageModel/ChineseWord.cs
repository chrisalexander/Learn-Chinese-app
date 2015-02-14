using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LanguageModel
{
    /// <summary>
    /// A Chinese word is formed of multiple characters.
    /// </summary>
    public class ChineseWord
    {
        /// <summary>
        /// Creates a new word from the traditional, simplified and pinyin representations.
        /// </summary>
        /// <param name="traditional">The traditional characters.</param>
        /// <param name="simplified">The simplified characters.</param>
        /// <param name="pinyin">The pinyin representation, each character separated by a space.</param>
        public ChineseWord(string traditional, string simplified, string pinyin)
        {
            var pinyinCharacters = pinyin.Split(' ');

            // Check all are of equal length
            // TODO this is going to barf on names with dots in them, punctuation, etc.
            if (traditional.Length != simplified.Length || simplified.Length != pinyinCharacters.Length)
            {
                throw new ArgumentException("Traditional, simplified and pinyin representations must all be the same length to be a valid Chinese word");
            }

            var characters = new List<ChineseCharacter>();

            for (var i = 0; i < traditional.Length; i++)
            {
                var character = new ChineseCharacter(traditional[i], simplified[i], pinyinCharacters[i]);
                characters.Add(character);
            }

            this.Characters = characters;
        }

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="characters">The characters.</param>
        [JsonConstructor]
        private ChineseWord(IEnumerable<ChineseCharacter> characters)
        {
            this.Characters = characters;
        }

        /// <summary>
        /// The characters in the word.
        /// </summary>
        public IEnumerable<ChineseCharacter> Characters { get; private set; }
    }
}
