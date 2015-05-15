namespace SimpleInMemorySearch
{
    /// <summary>
    /// Keyword associated with a particular item, along
    /// with its overall relevancy ranking.
    /// </summary>
    public class ScoredItemKeyword
    {
        /// <summary>
        /// Create a new keyword with score.
        /// </summary>
        /// <param name="keyword">The keyword.</param>
        /// <param name="score">The score.</param>
        public ScoredItemKeyword(string keyword, int score)
        {
            Score = score;
            Keyword = keyword;
        }

        /// <summary>
        /// The keyword.
        /// </summary>
        public string Keyword { get; private set; }

        /// <summary>
        /// The score of the keyword.
        /// </summary>
        public int Score { get; private set; }
    }
}
