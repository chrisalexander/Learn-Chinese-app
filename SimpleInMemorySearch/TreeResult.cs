using System.Collections.Generic;

namespace SimpleInMemorySearch
{
    /// <summary>
    /// Result of searching the tree.
    /// </summary>
    /// <typeparam name="T">The type of the object that is searched for.</typeparam>
    /// <typeparam name="TZ">The type of the value at each tree node.</typeparam>
    public class TreeResult<T, TZ> : ITreeResult<T, TZ>
    {
        /// <summary>
        /// Create a new result.
        /// </summary>
        /// <param name="match">The match phrase.</param>
        /// <param name="result">The matching object.</param>
        /// <param name="score">The score associated with the match.</param>
        public TreeResult(IEnumerable<TZ> match, T result, double score)
        {
            this.Match = match;
            this.Result = result;
            this.Score = score;
        }

        /// <summary>
        /// The match phrase.
        /// </summary>
        public IEnumerable<TZ> Match { get; private set; }

        /// <summary>
        /// The matching object.
        /// </summary>
        public T Result { get; private set; }

        /// <summary>
        /// The score associated with the match.
        /// </summary>
        public double Score { get; private set; }
    }
}
