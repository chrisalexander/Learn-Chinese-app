using DBUtils.Model;
using LongRunningProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace DBUtils.Services
{
    /// <summary>
    /// Interface for wrappers which use a file service and expose a user-safe way of accessing it.
    /// </summary>
    /// <typeparam name="Z">The concrete type of the database to expose.</typeparam>
    /// <typeparam name="T">The interface type of the database.</typeparam>
    public interface IFileServiceWrapper<Z, T> : ICommonFileService<T> where Z : T where T : IDatabase
    {
        /// <summary>
        /// Create a database file, asking the user for the location, with the specified filename.
        /// </summary>
        /// <param name="fileName">The name of the file to create.</param>
        /// <param name="process">The process.</param>
        /// <returns>The created storage file.</returns>
        Task<IStorageFile> CreateAsync(string fileName, IProcess process);

        /// <summary>
        /// Allow the user to specify a database file to open.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <returns>The loaded database.</returns>
        Task<Z> OpenAsync(IProcess process);

        /// <summary>
        /// Allow the user to specify a database file to open but not parse.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <returns>The loaded database.</returns>
        Task<IStorageFile> OpenWithoutParseAsync(IProcess process);

        /// <summary>
        /// Load the database in the specified file.
        /// </summary>
        /// <param name="file">The file to load the database from.</param>
        /// <param name="process">The process.</param>
        /// <returns>The loaded database.</returns>
        Task<Z> LoadAsync(IStorageFile file, IProcess process);

        /// <summary>
        /// Save the database to a file of the user's choosing.
        /// </summary>
        /// <param name="database">The database to save.</param>
        /// <param name="fileName">The name of the database file to save.</param>
        /// <param name="process">The process.</param>
        /// <returns>When complete.</returns>
        Task SaveAsync(T database, string fileName, IProcess process);
    }
}
