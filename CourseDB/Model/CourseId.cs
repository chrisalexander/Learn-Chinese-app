using System;
using DBUtils.Model;

namespace CourseDB.Model
{
    /// <summary>
    /// A course ID.
    /// </summary>
    public class CourseId : AbstractId
    {
        /// <summary>
        /// Create a new course ID.
        /// </summary>
        public CourseId()
        {
            this.RootId = Guid.NewGuid();
        }
    }
}
