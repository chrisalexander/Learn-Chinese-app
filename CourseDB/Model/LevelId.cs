using System;

namespace CourseDB.Model
{
    /// <summary>
    /// A level ID.
    /// </summary>
    public class LevelId
    {
        /// <summary>
        /// The ID of the course the level belongs to.
        /// </summary>
        public CourseId CourseId { get; set; }

        /// <summary>
        /// The ID of the level.
        /// </summary>
        public Guid Id { get; set; }
    }
}
