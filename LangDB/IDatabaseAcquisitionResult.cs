using System;

namespace LangDB
{
    /// <summary>
    /// Interface for acquisition results.
    /// </summary>
    public interface IDatabaseAcquisitionResult
    {
        /// <summary>
        /// The number of entries added.
        /// </summary>
        int Added { get; }

        /// <summary>
        /// The number of modified entries.
        /// </summary>
        int Modified { get; }

        /// <summary>
        /// The number of removed entries.
        /// </summary>
        int Removed { get; }

        /// <summary>
        /// The number of unmodified entries.
        /// </summary>
        int Unmodified { get; }

        /// <summary>
        /// The number of entries that used to be in the database.
        /// </summary>
        int OldTotal { get; }

        /// <summary>
        /// The total number of entries now in the database.
        /// </summary>
        int NewTotal { get; }
    }
}
