﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using LongRunningProcess;

namespace DBUtils.Windows
{
    /// <summary>
    /// Interface for services which can work with database files.
    /// </summary>
    /// <typeparam name="T">The type of the database.</typeparam>
    public interface IDatabaseFileService<in T> : ICommonFileService<T> where T : IDatabase
    {
        /// <summary>
        /// Create a database file, asking the user for the location, with the specified filename.
        /// </summary>
        /// <param name="fileName">The name of the file to create.</param>
        /// <param name="location">The default location for the picker.</param>
        /// <param name="process">The process.</param>
        /// <returns>The created storage file.</returns>
        Task<IStorageFile> CreateAsync(string fileName, PickerLocationId location, IProcess process);

        /// <summary>
        /// Allow the user to specify a database file to open.
        /// </summary>
        /// <typeparam name="TZ">The concrete type to load in to.</typeparam>
        /// <param name="extensions">List of supported extensions to filter on.</param>
        /// <param name="location">The default start location.</param>
        /// <param name="process">The process.</param>
        /// <returns>The loaded database.</returns>
        Task<TZ> OpenAsync<TZ>(IEnumerable<string> extensions, PickerLocationId location, IProcess process) where TZ : T;

        /// <summary>
        /// Allow the user to specify a database file to open but not parse.
        /// </summary>
        /// <param name="extensions">List of supported extensions to filter on.</param>
        /// <param name="location">The default start location.</param>
        /// <param name="process">The process.</param>
        /// <returns>The loaded database.</returns>
        Task<IStorageFile> OpenAsync(IEnumerable<string> extensions, PickerLocationId location, IProcess process);

        /// <summary>
        /// Load the database in the specified file.
        /// </summary>
        /// <typeparam name="TZ">The concrete type to load in to.</typeparam>
        /// <param name="file">The file to load the database from.</param>
        /// <param name="process">The process.</param>
        /// <returns>The loaded database.</returns>
        Task<TZ> LoadAsync<TZ>(IStorageFile file, IProcess process) where TZ : T;

        /// <summary>
        /// Save the database to a file of the user's choosing.
        /// </summary>
        /// <param name="database">The database to save.</param>
        /// <param name="fileName">The name of the database file to save.</param>
        /// <param name="location">The default location for the picker.</param>
        /// <param name="process">The process.</param>
        /// <returns>When complete.</returns>
        Task SaveAsync(T database, string fileName, PickerLocationId location, IProcess process);
    }
}
