using DBUtils;

namespace CourseDB
{
    /// <summary>
    /// An entry ID.
    /// </summary>
    public class EntryId : DescendantAbstractId<LevelId>
    {
        /// <summary>
        /// Create a new entry ID.
        /// </summary>
        /// <param name="levelId">The parent level ID.</param>
        public EntryId(LevelId levelId)
        {
            this.ParentId = levelId;
        }
    }
}
