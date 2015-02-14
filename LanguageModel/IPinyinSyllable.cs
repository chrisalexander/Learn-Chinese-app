
namespace LanguageModel
{
    /// <summary>
    /// Interface for Pinyin representation.
    /// </summary>
    public interface IPinyinSyllable
    {
        /// <summary>
        /// Letters in the syllable.
        /// </summary>
        string Letters { get; }

        /// <summary>
        /// The tone of the syllable.
        /// </summary>
        Tone Tone { get; }

        /// <summary>
        /// Convert the syllable to a single string.
        /// </summary>
        /// <returns>The string representation.</returns>
        string ToString();
    }
}
