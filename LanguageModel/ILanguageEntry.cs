using System.Collections.Generic;
using KeyedItemCollection;

namespace LanguageModel
{
    /// <summary>
    /// Interface for entries in the model.
    /// </summary>
    public interface ILanguageEntry : IKeyedItem<LanguageEntryId>
    {
        /// <summary>
        /// The Chinese format.
        /// </summary>
        IChineseWord Chinese { get; }

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
        LanguageEntryId Id { get; }
    }
}
