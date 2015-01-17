using LanguageModel;
using System.Collections.Generic;

namespace LangDB
{
    /// <summary>
    /// A language database contains the language data as well as information about its storage.
    /// </summary>
    public class LanguageDatabase
    {
        /// <summary>
        /// Construct a language database.
        /// </summary>
        /// <param name="path"></param>
        public LanguageDatabase(string path)
        {
            this.Path = path;
            this.Entries = new List<LanguageEntry>();
        }

        /// <summary>
        /// The entries in the database.
        /// </summary>
        public IList<LanguageEntry> Entries { get; private set; }

        /// <summary>
        /// The path to the database file.
        /// </summary>
        public string Path { get; private set; }
    }
}
