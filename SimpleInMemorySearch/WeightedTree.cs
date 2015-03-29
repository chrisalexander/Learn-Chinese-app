using System.Collections.Generic;

namespace SimpleInMemorySearch
{
    /// <summary>
    /// Weighted tree implementation.
    /// </summary>
    /// <typeparam name="T">Type of the object that is searched for.</typeparam>
    /// <typeparam name="TZ">Type of the value at each node.</typeparam>
    public class WeightedTree<T, TZ> : IWeightedTree<T, TZ>
    {
        /// <summary>
        /// Create a new weighted tree.
        /// </summary>
        public WeightedTree()
        {
            this.Edges = new Dictionary<TZ, IWeightedTree<T, TZ>>();
            this.Weight = 0;
            this.Items = new HashSet<T>();
        }

        /// <summary>
        /// The edges from this tree.
        /// </summary>
        public IDictionary<TZ, IWeightedTree<T, TZ>> Edges { get; private set; }

        /// <summary>
        /// The weight of the link to this node.
        /// </summary>
        public int Weight { get; private set; }

        /// <summary>
        /// The items that terminate at this node.
        /// </summary>
        public HashSet<T> Items { get; private set; } 

        /// <summary>
        /// Index the character array with the specified weight (defaulting to 1).
        /// </summary>
        /// <param name="nodeValues">The array of node values to index.</param>
        /// <param name="indexObject">The object to index at the location.</param>
        /// <param name="weight">The weight to assign to the array, default 1.</param>
        /// <returns>When complete.</returns>
        public void Index(IEnumerator<TZ> nodeValues, T indexObject, int weight = 1)
        {
            // If we have reached the end of the weighting, add the index object to our item collection
            if (!nodeValues.MoveNext())
            {
                this.Items.Add(indexObject);
                return;
            }

            // Add the weight on for the result
            this.Weight += weight;

            var nextValue = nodeValues.Current;

            if (!this.Edges.ContainsKey(nextValue))
            {
                this.Edges[nextValue] = new WeightedTree<T, TZ>();
            }

            // Index the rest of the enumerable down the tree.
            this.Edges[nextValue].Index(nodeValues, indexObject, weight);
        }

        /// <summary>
        /// Search the tree with the given prefix, returning all matching results.
        /// </summary>
        /// <param name="prefix">The prefix nodes.</param>
        /// <param name="previousKeys">The previous keys searched so far.</param>
        /// <param name="cumulativeWeight">The cumulative weight acquired so far.</param>
        /// <returns>The matched results, unsorted.</returns>
        public IEnumerable<ITreeResult<T, TZ>> Search(IEnumerator<TZ> prefix, IReadOnlyList<TZ> previousKeys = null, int cumulativeWeight = 0)
        {
            var updatedKeys = new List<TZ>(previousKeys);

            // We have reached the end of the prefix, so return all values from here
            if (!prefix.MoveNext())
            {
                foreach (var item in this.AllItems(previousKeys, cumulativeWeight))
                {
                    yield return item;
                }
            }

            var nextValue = prefix.Current;

            cumulativeWeight += this.Weight;
            updatedKeys.Add(nextValue);

            if (!this.Edges.ContainsKey(nextValue))
            {
                yield break;
            }

            // Search the found node for the rest of the search prefix
            foreach (var result in this.Edges[nextValue].Search(prefix, updatedKeys, cumulativeWeight))
            {
                yield return result;
            }
        }

        public IEnumerable<ITreeResult<T, TZ>> AllItems(IReadOnlyList<TZ> previousKeys, int cumulativeWeight)
        {
            cumulativeWeight += this.Weight;

            // Return all this node's items
            foreach (var item in this.Items)
            {
                yield return new TreeResult<T, TZ>(previousKeys, item, cumulativeWeight / (double)previousKeys.Count);
            }

            // Return all child node's items
            foreach (var child in this.Edges)
            {
                var updatedKeys = new List<TZ>(previousKeys);
                updatedKeys.Add(child.Key);

                foreach (var childItem in child.Value.AllItems(updatedKeys, cumulativeWeight))
                {
                    yield return childItem;
                }
            }
        }
    }
}
