
namespace LangDB.Model
{
    /// <summary>
    /// Communicates the results of a database acquisition.
    /// </summary>
    public class DatabaseAcquisitionResult : IDatabaseAcquisitionResult
    {
        /// <summary>
        /// The number of entries added.
        /// </summary>
        public int Added { get; set; }

        /// <summary>
        /// The number of entries removed.
        /// </summary>
        public int Removed { get; set; }

        /// <summary>
        /// The number of entries modified.
        /// </summary>
        public int Modified { get; set; }

        /// <summary>
        /// The number of unmodified entries.
        /// </summary>
        public int Unmodified { get; set; }

        /// <summary>
        /// The number of entries that used to be in the database.
        /// </summary>
        public int OldTotal { get; set; }

        /// <summary>
        /// The total number of entries now in the database.
        /// </summary>
        public int NewTotal { get; set; }
    }
}
