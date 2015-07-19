using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeyedItemCollection;

namespace SimpleInMemorySearch
{
    /// <summary>
    /// Abstract base class for search services. Implementers must
    /// provide type-specific details on how to index the type.
    /// </summary>
    /// <typeparam name="T">The type of the object to search.</typeparam>
    /// <typeparam name="TZ">The type of the object key.</typeparam>
    public abstract class AbstractSearchService<T, TZ> : ISearchService<T, TZ> where T : IKeyedItem<TZ>
    {
        /// <summary>
        /// Cache of all items to search.
        /// </summary>
        private readonly IKeyedItemCollection<T, TZ> itemCache;

        /// <summary>
        /// The weighted chharacter tree of words.
        /// </summary>
        private readonly IWeightedTree<TZ, char> weightedTree;

        /// <summary>
        /// Create a new search service.
        /// </summary>
        protected AbstractSearchService()
        {
            this.itemCache = new KeyedItemCollection<T, TZ>();
            this.weightedTree = new WeightedTree<TZ, char>();
        }

        /// <summary>
        /// Adds the item to the index.
        /// </summary>
        /// <param name="item">The item to index.</param>
        public async Task Index(T item)
        {
            // Make sure the item is in the item cache, or update it
            this.itemCache.Add(item);

            // Get all of the keywords from the item
            var keywords = this.Keywords(item);

            // Create all the tasks to index the terms
            await Task.Run(() =>
            {
                foreach (var keyword in keywords)
                {
                    this.weightedTree.Index(keyword.Keyword.ToCharArray(), item.Key, keyword.Score);
                }
            });
        }

        /// <summary>
        /// Asynchronously search for results relating to the specified term.
        /// </summary>
        /// <param name="termString">The search term.</param>
        /// <returns>The search results matching the term.</returns>
        public async Task<IEnumerable<ISearchResult<T, TZ>>> Search(string termString)
        {
            var terms = termString.Split(' ');

            var overallResults = new Dictionary<TZ, IResultTracker<TZ>>();

            var firstTerm = true;

            foreach (var term in terms)
            {
                var results = this.weightedTree.Search(term.ToCharArray());

                var resultIds = new HashSet<TZ>();

                // Add the results to the overall results
                foreach (var result in results)
                {
                    resultIds.Add(result.Result);

                    IResultTracker<TZ> resultTracker;
                    if (overallResults.TryGetValue(result.Result, out resultTracker))
                    {
                        resultTracker.Score += result.Score;
                    }
                    else if (firstTerm)
                    {
                        overallResults.Add(result.Result, new ResultTracker<TZ>(result.Result, result.Score));
                    }
                }

                // Remove from the overall results, entries which were not found in this set of results
                var overallToRemove = new HashSet<TZ>();
                
                foreach (var overallResult in overallResults)
                {
                    if (!resultIds.Contains(overallResult.Key))
                    {
                        overallToRemove.Add(overallResult.Key);
                    }
                }

                foreach (var toRemove in overallToRemove)
                {
                    overallResults.Remove(toRemove);
                }

                firstTerm = false;
            }

            return await Task.Run(() => overallResults.Select(r => new SearchResult<T, TZ>(r.Value.Result, r.Value.Score, tz => this.itemCache[tz])).OrderBy(r => r.Relevancy));
        }

        /// <summary>
        /// Gets an object from its key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The object.</returns>
        public T ObjectFromKey(TZ key)
        {
            return this.itemCache[key];
        }

        /// <summary>
        /// Given an item, return keywords from the item with relative rankings of their score.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>Keywords and their scores.</returns>
        protected abstract IEnumerable<ScoredItemKeyword> Keywords(T item);
    }
}
