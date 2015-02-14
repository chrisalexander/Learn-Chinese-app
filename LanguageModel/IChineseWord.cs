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
        IEnumerable<ChineseCharacter> Characters { get; }
    }
}
