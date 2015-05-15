using System;
using KeyedItemCollection;

namespace SimpleInMemorySearch
{
    /// <summary>
    /// Defines the interface for a search result.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <typeparam name="TZ">The type of the result's key.</typeparam>
    public interface ISearchResult<T, out TZ> where T : IKeyedItem<TZ>
    {
        /// <summary>
        /// The relevancy of the result.
        /// </summary>
        double Relevancy { get; }

        /// <summary>
        /// The key of the result.
        /// </summary>
        TZ Key { get; }

        /// <summary>
        /// The result itself.
        /// </summary>
        Lazy<T> Result { get; } 
    }
}
