namespace SimpleInMemorySearch
{
    /// <summary>
    /// Interface for tracking the details of results from a search.
    /// </summary>
    /// <typeparam name="T">The type of the object that is searched for.</typeparam>
    public interface IResultTracker<out T>
    {
        /// <summary>
        /// The matching object.
        /// </summary>
        T Result { get; }

        /// <summary>
        /// The score associated with the match.
        /// </summary>
        double Score { get; set; }
    }
}
