using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleInMemorySearch
{
    /// <summary>
    /// Interface for a search tree where elements are linked by weight.
    /// </summary>
    /// <typeparam name="T">The type of the object that is searched for.</typeparam>
    /// <typeparam name="TZ">The type of the value at each tree node.</typeparam>
    public interface IWeightedTree<T, TZ>
    {
        /// <summary>
        /// Index the character array with the specified weight (defaulting to 1).
        /// </summary>
        /// <param name="nodeValues">The array of node values to index.</param>
        /// <param name="indexObject">The object to index at the location.</param>
        /// <param name="weight">The weight to assign to the array, default 1.</param>
        /// <returns>When complete.</returns>
        Task Index(IEnumerable<TZ> nodeValues, T indexObject, int weight = 1);

        /// <summary>
        /// Search the tree with the given prefix, returning all matching results.
        /// </summary>
        /// <param name="prefix">The prefix nodes.</param>
        /// <returns>The matched results, sorted by highest weighting first.</returns>
        Task<IEnumerable<ITreeResult<T, TZ>>> Search(IEnumerable<TZ> prefix);
    }
}
