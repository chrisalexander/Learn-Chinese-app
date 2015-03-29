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
        /// The edges from this tree.
        /// </summary>
        IDictionary<TZ, IWeightedTree<T, TZ>> Edges { get; }

        /// <summary>
        /// The weight of the link to this node.
        /// </summary>
        int Weight { get; }

        /// <summary>
        /// The items that terminate at this node.
        /// </summary>
        HashSet<T> Items { get; } 

        /// <summary>
        /// Index the character array with the specified weight (defaulting to 1).
        /// </summary>
        /// <param name="nodeValues">The array of node values to index.</param>
        /// <param name="indexObject">The object to index at the location.</param>
        /// <param name="weight">The weight to assign to the array, default 1.</param>
        /// <returns>When complete.</returns>
        void Index(IEnumerator<TZ> nodeValues, T indexObject, int weight = 1);

        /// <summary>
        /// Search the tree with the given prefix, returning all matching results.
        /// </summary>
        /// <param name="prefix">The prefix nodes.</param>
        /// <param name="previousKeys">The previous keys searched so far.</param>
        /// <param name="cumulativeWeight">The cumulative weight acquired so far.</param>
        /// <returns>The matched results, unsorted.</returns>
        IEnumerable<ITreeResult<T, TZ>> Search(IEnumerator<TZ> prefix, IReadOnlyList<TZ> previousKeys = null, int cumulativeWeight = 0);

        /// <summary>
        /// Recursively retrieve all items from this node down the tree.
        /// </summary>
        /// <param name="previousKeys">The previous keys searched so far.</param>
        /// <param name="cumulativeWeight">The cumulative weight acquired so far.</param>
        /// <returns>All items, unsorted.</returns>
        IEnumerable<ITreeResult<T, TZ>> AllItems(IReadOnlyList<TZ> previousKeys, int cumulativeWeight);
    }
}
