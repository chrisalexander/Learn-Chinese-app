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

        /// <summary>
        /// Get a string representation of the ID.
        /// </summary>
        /// <returns>String representation of the ID.</returns>
        public override string ToString()
        {
            return "Level: " + this.Id + "; " + this.CourseId;
        }
    }
}
