using LanguageModel;
using System.Collections.Generic;

namespace LangDB
{
    /// <summary>
    /// A language database contains the language data as well as information about its storage.
    /// </summary>
    public class LanguageDatabase : ILanguageDatabase
    {
        /// <summary>
        /// Construct a language database.
        /// </summary>
        public LanguageDatabase()
        {
            this.Entries = new List<LanguageEntry>();
        }

        /// <summary>
        /// The entries in the database.
        /// </summary>
        public IList<LanguageEntry> Entries { get; private set; }

        /// <summary>
        /// The path to the database file.
        /// </summary>
        public string Path { get; set; }
    }
}
