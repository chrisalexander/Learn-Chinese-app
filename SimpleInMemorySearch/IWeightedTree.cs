using System.Collections.Generic;

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
        IDictionary<T, double> Items { get; }

        /// <summary>
        /// Index the key enumeration with the specified weight (defaulting to 1).
        /// </summary>
        /// <param name="nodeValues">The array of node values to index.</param>
        /// <param name="indexObject">The object to index at the location.</param>
        /// <param name="weight">The weight to assign to the array, default 1.</param>
        /// <returns>When complete.</returns>
        void Index(IEnumerable<TZ> nodeValues, T indexObject, int weight = 1);

        /// <summary>
        /// Index the key enumerator with the specified weight (defaulting to 1).
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
        /// <returns>The matched results, unsorted and potentially duplicated.</returns>
        IEnumerable<ITreeResult<T, TZ>> Search(IEnumerable<TZ> prefix);

        /// <summary>
        /// Search the tree with the given prefix, previous key information and cumulative weight, returning all matching results.
        /// </summary>
        /// <param name="prefix">The prefix nodes.</param>
        /// <param name="previousKeys">The previous keys searched so far.</param>
        /// <param name="cumulativeWeight">The cumulative weight acquired so far.</param>
        /// <returns>The matched results, unsorted and potentially duplicated.</returns>
        IEnumerable<ITreeResult<T, TZ>> Search(IEnumerator<TZ> prefix, IReadOnlyList<TZ> previousKeys = null, int cumulativeWeight = 0);

        /// <summary>
        /// Recursively retrieve all items from this node down the tree.
        /// </summary>
        /// <returns>All items, unsorted and potentially duplicated.</returns>
        IEnumerable<ITreeResult<T, TZ>> AllItems();

        /// <summary>
        /// Recursively retrieve all items from this node down the tree, with the given previous keys.
        /// </summary>
        /// <param name="previousKeys">The previous keys searched so far.</param>
        /// <returns>All items, unsorted and potentially duplicated.</returns>
        IEnumerable<ITreeResult<T, TZ>> AllItems(IReadOnlyList<TZ> previousKeys);
    }
}
