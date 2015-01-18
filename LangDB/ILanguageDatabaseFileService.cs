using LongRunningProcess;
using System.Threading.Tasks;
using Windows.Storage;

namespace LangDB
{
    /// <summary>
    /// Interface for services which can work with database files.
    /// </summary>
    public interface ILanguageDatabaseFileService
    {
        /// <summary>
        /// Load the language database in the specified file.
        /// </summary>
        /// <param name="file">The file to load the database from.</param>
        /// <returns>The loaded database.</returns>
        Task<LanguageDatabase> LoadAsync(StorageFile file);

        /// <summary>
        /// Save the specified language database to the file.
        /// </summary>
        /// <param name="database">The database to save.</param>
        /// <param name="file">The file to save it to.</param>
        /// <param name="process">The process.</param>
        /// <returns>When complete.</returns>
        Task SaveAsync(LanguageDatabase database, StorageFile file, IProcess process);
    }
}
