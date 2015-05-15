using LanguageModel.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LanguageModel
{
    /// <summary>
    /// A language entry is a Chinese word and its English equivalents.
    /// </summary>
    public class LanguageEntry : ILanguageEntry
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

            PopulateId();
        }

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="chinese">The Chinese word.</param>
        /// <param name="english">The English phrases.</param>
        [JsonConstructor, UsedImplicitly]
        private LanguageEntry(ChineseWord chinese, IEnumerable<string> english)
        {
            this.Chinese = chinese;
            this.English = english;

            PopulateId();
        }

        /// <summary>
        /// ID of the entry, which is formed of the traditional + simplified characters.
        /// </summary>
        public LanguageEntryId Id { get; private set; }

        /// <summary>
        /// Gets the key.
        /// </summary>
        public LanguageEntryId Key
        {
            get
            {
                return this.Id;
            }
        }

        /// <summary>
        /// The hash of the entry's contents.
        /// </summary>
        public string Hash
        {
            get
            {
                var builder = new StringBuilder();

                foreach (var character in this.Chinese.Characters)
                {
                    builder.Append(character.Traditional);
                    builder.Append(character.Simplified);
                    builder.Append(character.Mandarin);
                }

                foreach (var english in this.English)
                {
                    builder.Append(english);
                }

                var keyBytes = Encoding.UTF8.GetBytes("Language Model");
                var hashAlgorithm = new HMACSHA1(keyBytes);
                byte[] dataBuffer = Encoding.UTF8.GetBytes(builder.ToString());
                byte[] hashBytes = hashAlgorithm.ComputeHash(dataBuffer);
                return Convert.ToBase64String(hashBytes);
            }
        }

        /// <summary>
        /// The Chinese word.
        /// </summary>
        public IChineseWord Chinese { get; private set; }

        /// <summary>
        /// The English translations and notes of the Chinese word.
        /// </summary>
        public IEnumerable<string> English { get; private set; }

        /// <summary>
        /// Helper to populate the ID.
        /// </summary>
        private void PopulateId()
        {
            var builder = new StringBuilder();

            foreach (var character in this.Chinese.Characters)
            {
                builder.Append(character.Traditional);
                builder.Append(character.Simplified);
            }

            this.Id = new LanguageEntryId(builder.ToString());
        }
    }
}
