using System;
using System.Runtime.InteropServices.WindowsRuntime;
using KeyedItemCollection;

namespace SimpleInMemorySearch
{
    /// <summary>
    /// A search result.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <typeparam name="TZ">The type of the result key.</typeparam>
    public class SearchResult<T, TZ> : ISearchResult<T, TZ> where T : IKeyedItem<TZ>
    {
        /// <summary>
        /// Create a new search result.
        /// </summary>
        /// <param name="key">The key of the result.</param>
        /// <param name="relevancy">The overall relevancy.</param>
        /// <param name="accessorFunction">Function to access a result object from the key.</param>
        public SearchResult(TZ key, int relevancy, Func<TZ, T> accessorFunction)
        {
            this.Relevancy = relevancy;
            this.Key = key;

            this.Result = new Lazy<T>(() => accessorFunction(this.Key));
        }

        /// <summary>
        /// The relevancy of the result.
        /// </summary>
        public int Relevancy { get; private set; }

        /// <summary>
        /// The key of the result.
        /// </summary>
        public TZ Key { get; private set; }

        /// <summary>
        /// The result itself.
        /// </summary>
        public Lazy<T> Result { get; private set; }
    }
}
