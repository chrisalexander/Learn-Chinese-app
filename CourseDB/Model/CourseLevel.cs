using System;
using System.Collections.Generic;

namespace CourseDB.Model
{
    /// <summary>
    /// A level in a course.
    /// </summary>
    public class CourseLevel
    {
        /// <summary>
        /// Construct a course level.
        /// </summary>
        public CourseLevel()
        {
            this.EntryIds = new List<string>();
        }

        /// <summary>
        /// The level ID.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the level.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the level.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The difficulty of the level.
        /// </summary>
        public LevelDifficulty Difficulty { get; set; }

        /// <summary>
        /// The ordered list of IDs of language entries that form this level.
        /// </summary>
        public IList<string> EntryIds { get; private set; }
    }
}
