using System;

namespace CourseDB.Model
{
    /// <summary>
    /// A course ID.
    /// </summary>
    public class CourseId
    {
        /// <summary>
        /// The unique ID of the course.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Equals comparison.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>Whether the objects are the equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return this.Id.Equals(((CourseId)obj).Id);
        }

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
