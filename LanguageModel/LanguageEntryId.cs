using DBUtils;

namespace LanguageModel
{
    /// <summary>
    /// Language entry ID.
    /// </summary>
    public class LanguageEntryId : AbstractId
    {
        /// <summary>
        /// Create a new language entry ID.
        /// </summary>
        /// <param name="rootId">The entry ID.</param>
        public LanguageEntryId(string rootId) : base(rootId) { }
    }
}
