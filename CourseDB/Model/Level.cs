using System.Collections.Generic;

namespace CourseDB.Model
{
    /// <summary>
    /// A level in a course.
    /// </summary>
    public class Level : ILevel
    {
        /// <summary>
        /// Construct a course level.
        /// </summary>
        public Level()
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
        public IList<LevelEntry> Entries { get; set; }

        /// <summary>
        /// List of IDs prerequisite levels for this level.
        /// </summary>
        public IList<LevelId> Prerequisites { get; set; }
    }
}
