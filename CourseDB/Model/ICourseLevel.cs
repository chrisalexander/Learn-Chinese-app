using System.Collections.Generic;

namespace CourseDB.Model
{
    /// <summary>
    /// Interface for levels.
    /// </summary>
    public interface ICourseLevel
    {
        /// <summary>
        /// Descripton of the level.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Difficulty of the level.
        /// </summary>
        LevelDifficulty Difficulty { get; set; }

        /// <summary>
        /// Entries in the level.
        /// </summary>
        IList<LevelEntry> Entries { get; }

        /// <summary>
        /// ID of the level.
        /// </summary>
        LevelId Id { get; set; }

        /// <summary>
        /// Name of the level.
        /// </summary>
        string Name { get; set; }
        
        /// <summary>
        /// The level's prerequisites.
        /// </summary>
        IList<LevelId> Prerequisites { get; }
    }
}
