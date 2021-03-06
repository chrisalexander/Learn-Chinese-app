﻿using System.Text;
using LanguageModel.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LanguageModel
{
    /// <summary>
    /// A Chinese word is formed of multiple characters.
    /// </summary>
    public class ChineseWord : IChineseWord
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
        [JsonConstructor, UsedImplicitly]
        private ChineseWord(IEnumerable<ChineseCharacter> characters)
        {
            this.Characters = characters;
        }

        /// <summary>
        /// The characters in the word.
        /// </summary>
        public IEnumerable<IChineseCharacter> Characters { get; private set; }

        /// <summary>
        /// The mandarin chinese.
        /// </summary>
        public string Mandarin
        {
            get
            {
                var builder = new StringBuilder();

                foreach (var character in this.Characters)
                {
                    builder.Append(character.Mandarin);
                    builder.Append(" ");
                }

                return builder.ToString();
            }
        }

        /// <summary>
        /// The traditional chinese.
        /// </summary>
        public string Traditional
        {
            get
            {
                var builder = new StringBuilder();

                foreach (var character in this.Characters)
                {
                    builder.Append(character.Traditional);
                }

                return builder.ToString();
            }
        }

        /// <summary>
        /// The simplified chinese.
        /// </summary>
        public string Simplified
        {
            get
            {
                var builder = new StringBuilder();

                foreach (var character in this.Characters)
                {
                    builder.Append(character.Simplified);
                }

                return builder.ToString();
            }
        }

        /// <summary>
        /// Converts a Chinese word to a string.
        /// </summary>
        /// <returns>String representation fo the word.</returns>
        public override string ToString()
        {
            return this.Simplified;
        }
    }
}
