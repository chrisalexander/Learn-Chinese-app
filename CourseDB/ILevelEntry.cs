namespace CourseDB
{
    /// <summary>
    /// Interface for level entries.
    /// </summary>
    public interface ILevelEntry
    {
        /// <summary>
        /// ID of the language entry.
        /// </summary>
        EntryId Id { get; set; }

        /// <summary>
        /// The translation chosen to use for the entry in the level.
        /// </summary>
        string SelectedTranslation { get; set; }
    }
}
