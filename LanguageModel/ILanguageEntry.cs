using System.Collections.Generic;

namespace LanguageModel
{
    /// <summary>
    /// Interface for entries in the model.
    /// </summary>
    public interface ILanguageEntry
    {
        /// <summary>
        /// The Chinese format.
        /// </summary>
        ChineseWord Chinese { get; }
        
        /// <summary>
        /// The English translations.
        /// </summary>
        IEnumerable<string> English { get; }

        /// <summary>
        /// The hash of the entry.
        /// </summary>
        string Hash { get; }

        /// <summary>
        /// The ID of the entry.
        /// </summary>
        string Id { get; }
    }
}
