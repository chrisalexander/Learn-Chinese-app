using DBUtils.Model;
using LongRunningProcess;
using System.Threading.Tasks;
using Windows.Storage;

namespace DBUtils.Services
{
    /// <summary>
    /// Interface for services which can work with database files.
    /// </summary>
    /// <typeparam name="T">The type of the database.</typeparam>
    public interface IDatabaseFileService<T> where T : IDatabase
    {
        /// <summary>
        /// Load the database in the specified file.
        /// </summary>
        /// <typeparam name="Z">The concrete type to load in to.</typeparam>
        /// <param name="file">The file to load the database from.</param>
        /// <param name="process">The process.</param>
        /// <returns>The loaded database.</returns>
        Task<Z> LoadAsync<Z>(IStorageFile file, IProcess process) where Z : T;

        /// <summary>
        /// Save the specified database to the file.
        /// </summary>
        /// <param name="database">The database to save.</param>
        /// <param name="file">The file to save it to.</param>
        /// <param name="process">The process.</param>
        /// <returns>When complete.</returns>
        Task SaveAsync(T database, IStorageFile file, IProcess process);
    }
}
