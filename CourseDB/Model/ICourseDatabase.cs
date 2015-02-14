﻿using DBUtils.Model;
using System;
using System.Collections.Generic;

namespace CourseDB.Model
{
    /// <summary>
    /// The course database interface.
    /// </summary>
    public interface ICourseDatabase : IDatabase
    {
        /// <summary>
        /// ID of the course.
        /// </summary>
        CourseId Id { get; set; }

        /// <summary>
        /// The name of the course.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Description of the course.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// When the course was last updated.
        /// </summary>
        DateTime Updated { get; set; }

        /// <summary>
        /// The path to the database file.
        /// </summary>
        string Path { get; set; }

        /// <summary>
        /// The course's levels.
        /// </summary>
        IList<ICourseLevel> Levels { get; }
        
    }
}
