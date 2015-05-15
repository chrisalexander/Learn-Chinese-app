using System.Collections.Generic;
using System.Threading.Tasks;
using KeyedItemCollection;

namespace SimpleInMemorySearch
{
    /// <summary>
    /// Interface for search services.
    /// </summary>
    /// <typeparam name="T">The type of the object to search.</typeparam>
    /// <typeparam name="TZ">The type of the object key.</typeparam>
    public interface ISearchService<T, TZ> where T : IKeyedItem<TZ>
    {
        /// <summary>
        /// Adds the item to the index.
        /// </summary>
        /// <param name="item">The item to index.</param>
        Task Index(T item);

        /// <summary>
        /// Asynchronously search for results relating to the specified term.
        /// </summary>
        /// <param name="term">The search term.</param>
        /// <returns>The search results matching the term.</returns>
        Task<IEnumerable<ISearchResult<T, TZ>>> Search(string term);

        /// <summary>
        /// Get a specific object from its key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The associated object.</returns>
        T ObjectFromKey(TZ key);
    }
}
