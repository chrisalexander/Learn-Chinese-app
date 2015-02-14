using Newtonsoft.Json;
using System;

namespace LanguageModel
{
    /// <summary>
    /// Represents the pronunciation of a character in Pinyin form.
    /// </summary>
    public class PinyinSyllable : IPinyinSyllable
    {
        /// <summary>
        /// Constructs the syllable from the complete Pinyin string.
        /// </summary>
        /// <param name="complete">The letters and the numbers as one input.</param>
        public PinyinSyllable(string complete)
        {
            if (complete.Length < 1)
            {
                throw new ArgumentOutOfRangeException("Complete must be at least one character");
            }

            var lastChar = complete[complete.Length - 1];

            try
            {
                var number = int.Parse(lastChar.ToString());
                this.Tone = (Tone)number;
                this.Letters = complete.Substring(0, complete.Length - 1);
            }
            catch (Exception)
            {
                // If we cannot convert then we assume there isn't one.
                this.Tone = Tone.Short;
                this.Letters = complete;
            }
        }

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="Letters">The letters.</param>
        /// <param name="Tone">The tone.</param>
        [JsonConstructor]
        private PinyinSyllable(string Letters, Tone Tone)
        {
            this.Letters = Letters;
            this.Tone = Tone;
        }

        /// <summary>
        /// Gets the characters of the syllable.
        /// </summary>
        public string Letters { get; private set; }

        /// <summary>
        /// Gets the tone of the syllable.
        /// </summary>
        public Tone Tone { get; private set; }

        /// <summary>
        /// Get the Pinyin form of the pronounciation.
        /// </summary>
        /// <returns>The pronounciation in Pinyin form.</returns>
        public override string ToString()
        {
            return this.Letters + ((int)this.Tone);
        }
    }
}
