using System.Collections;
using System.Collections.Generic;

namespace SimpleInMemorySearch
{
    /// <summary>
    /// Interface for results of searching the tree.
    /// </summary>
    /// <typeparam name="T">The type of the value at each tree node.</typeparam>
    public interface ITreeResult<out T>
    {
        /// <summary>
        /// The match itself.
        /// </summary>
        IEnumerable<T> Match { get; }

        /// <summary>
        /// The score associated with the match.
        /// </summary>
        int Score { get; }
    }
}
