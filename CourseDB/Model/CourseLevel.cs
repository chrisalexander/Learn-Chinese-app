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
            this.Entries = new List<LevelEntry>();
            this.Prerequisites = new List<LevelId>();
        }

        /// <summary>
        /// The level ID.
        /// </summary>
        public LevelId Id { get; set; }

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
        /// The ordered list of entries that form this level.
        /// </summary>
        public IList<LevelEntry> Entries { get; private set; }

        /// <summary>
        /// List of IDs prerequisite levels for this level.
        /// </summary>
        public IList<LevelId> Prerequisites { get; private set; }
    }
}
