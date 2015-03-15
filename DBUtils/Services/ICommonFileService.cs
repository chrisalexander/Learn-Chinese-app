using DBUtils.Model;
using LongRunningProcess;
using System.Threading.Tasks;
using Windows.Storage;

namespace DBUtils.Services
{
    /// <summary>
    /// Interface for methods that are common to both database file services and file service wrappers.
    /// </summary>
    public interface ICommonFileService<in T> where T : IDatabase
    {
        /// <summary>
        /// Create a new database file in the specified folder.
        /// </summary>
        /// <param name="folder">The folder to put the database file in.</param>
        /// <param name="fileName">The name of the file to create.</param>
        /// <param name="process">The process.</param>
        /// <returns>The created file.</returns>
        Task<IStorageFile> CreateAsync(IStorageFolder folder, string fileName, IProcess process);

        /// <summary>
        /// Save the specified database to a given file.
        /// </summary>
        /// <param name="database">The database to save.</param>
        /// <param name="file">The file to save it to.</param>
        /// <param name="process">The process.</param>
        /// <returns>When complete.</returns>
        Task SaveAsync(T database, IStorageFile file, IProcess process);

        /// <summary>
        /// Delete the specified database file.
        /// </summary>
        /// <param name="file">The file to delete.</param>
        /// <param name="process">The process.</param>
        /// <returns>When complete.</returns>
        Task DeleteAsync(IStorageFile file, IProcess process);
    }
}
