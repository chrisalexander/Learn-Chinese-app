namespace SimpleInMemorySearch
{
    /// <summary>
    /// Class for tracking the details of results from a search.
    /// </summary>
    /// <typeparam name="T">The type of the object that is searched for.</typeparam>
    public class ResultTracker<T> : IResultTracker<T>
    {
        /// <summary>
        /// Create a new Result Tracker.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="initialScore">The initial score.</param>
        public ResultTracker(T result, double initialScore)
        {
            this.Result = result;
            this.Score = initialScore;
        }

        /// <summary>
        /// The matching object.
        /// </summary>
        public T Result { get; private set; }

        /// <summary>
        /// The score associated with the match.
        /// </summary>
        public double Score { get; set; }
    }
}
