using System.Collections.Generic;

namespace LanguageModel
{
    /// <summary>
    /// Interface for Chinese words.
    /// </summary>
    public interface IChineseWord
    {
        /// <summary>
        /// The characters in the word.
        /// </summary>
        IEnumerable<IChineseCharacter> Characters { get; }

        /// <summary>
        /// The mandarin pinyin.
        /// </summary>
        string Mandarin { get; }

        /// <summary>
        /// The traditional chinese.
        /// </summary>
        string Traditional { get; }

        /// <summary>
        /// The simplified chinese.
        /// </summary>
        string Simplified { get; }
    }
}
