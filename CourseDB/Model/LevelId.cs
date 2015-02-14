using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
