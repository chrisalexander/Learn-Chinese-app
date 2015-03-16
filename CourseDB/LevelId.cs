using System;
using DBUtils;

namespace CourseDB
{
    /// <summary>
    /// A level ID.
    /// </summary>
    public class LevelId : DescendantAbstractId<CourseId>
    {
        /// <summary>
        /// Create a new level ID.
        /// </summary>
        /// <param name="courseId">The parent course ID.</param>
        public LevelId(CourseId courseId)
        {
            this.ParentId = courseId;
            this.RootId = Guid.NewGuid();
        }
    }
}
