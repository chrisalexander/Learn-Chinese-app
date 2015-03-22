using LanguageModel;

namespace CourseDB
{
    /// <summary>
    /// An entry within a level.
    /// </summary>
    public class LevelEntry : ILevelEntry
    {
        /// <summary>
        /// The ID of the entry in the entry database.
        /// </summary>
        public EntryId Id { get; set; }

        /// <summary>
        /// The ID of the language entry from the language database.
        /// </summary>
        public LanguageEntryId LanguageEntryId { get; set; }

        /// <summary>
        /// The selected translation to be used in the level.
        /// </summary>
        public string SelectedTranslation { get; set; }
    }
}
