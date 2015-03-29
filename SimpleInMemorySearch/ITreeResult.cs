using System.Collections.Generic;

namespace SimpleInMemorySearch
{
    /// <summary>
    /// Interface for results of searching the tree.
    /// </summary>
    /// <typeparam name="T">The type of the object that is searched for.</typeparam>
    /// <typeparam name="TZ">The type of the value at each tree node.</typeparam>
    public interface ITreeResult<T, out TZ>
    {
        /// <summary>
        /// The match phrase.
        /// </summary>
        IEnumerable<TZ> Match { get; }

        /// <summary>
        /// The matching object.
        /// </summary>
        T Result { get; }

        /// <summary>
        /// The score associated with the match.
        /// </summary>
        double Score { get; }
    }
}
