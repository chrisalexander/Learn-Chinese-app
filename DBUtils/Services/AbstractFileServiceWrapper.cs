﻿using DBUtils.Model;
using LongRunningProcess;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace DBUtils.Services
{
    /// <summary>
    /// Abstract class that implementers can use to easily create a default wrapper for a database file service.
    /// </summary>
    /// <typeparam name="Z">The concrete type of the database to expose on the API.</typeparam>
    /// <typeparam name="T">The interface type of the database.</typeparam>
    public abstract class AbstractFileServiceWrapper<Z, T> : IFileServiceWrapper<Z, T> where Z : T where T : IDatabase
    {
        /// <summary>
        /// Implementer has to provide the underlying file service for us to access.
        /// </summary>
        protected abstract IDatabaseFileService<T> FileService { get; }


        /// <summary>
        /// Implementer provides the default picker location.
        /// </summary>
        protected abstract PickerLocationId DefaultPickerLocation { get; }

        /// <summary>
        /// Implementer provides default extensions.
        /// </summary>
        protected abstract IEnumerable<string> Extensions { get; }

        #region ICommonFileService implementation

        /// <summary>
        /// Create a new database file in the specified folder.
        /// </summary>
        /// <param name="folder">The folder to put the database file in.</param>
        /// <param name="fileName">The name of the file to create.</param>
        /// <param name="process">The process.</param>
        /// <returns>The created file.</returns>
        public Task<IStorageFile> CreateAsync(IStorageFolder folder, string fileName, IProcess process)
        {
            return this.FileService.CreateAsync(folder, fileName, process);
        }

        /// <summary>
        /// Save the specified database to a given file.
        /// </summary>
        /// <param name="database">The database to save.</param>
        /// <param name="file">The file to save it to.</param>
        /// <param name="process">The process.</param>
        /// <returns>When complete.</returns>
        public Task SaveAsync(T database, IStorageFile file, IProcess process)
        {
            return this.FileService.SaveAsync(database, file, process);
        }

        /// <summary>
        /// Delete the specified database file.
        /// </summary>
        /// <param name="file">The file to delete.</param>
        /// <param name="process">The process.</param>
        /// <returns>When complete.</returns>
        public Task DeleteAsync(IStorageFile file, IProcess process)
        {
            return this.FileService.DeleteAsync(file, process);
        }

        #endregion

        #region IFileServiceWrapper implementation

        /// <summary>
        /// Create a database file, asking the user for the location, with the specified filename.
        /// </summary>
        /// <param name="fileName">The name of the file to create, without extension.</param>
        /// <param name="process">The process.</param>
        /// <returns>The created storage file.</returns>
        public Task<IStorageFile> CreateAsync(string fileName, IProcess process)
        {
            return this.FileService.CreateAsync(fileName + this.Extensions.First(), this.DefaultPickerLocation, process);
        }

        /// <summary>
        /// Allow the user to specify a database file to open.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <returns>The loaded database.</returns>
        public Task<Z> OpenAsync(IProcess process)
        {
            return this.FileService.OpenAsync<Z>(this.Extensions, this.DefaultPickerLocation, process);
        }

        /// <summary>
        /// Allow the user to specify a database file to open but not parse.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <returns>The loaded database.</returns>
        public Task<IStorageFile> OpenWithoutParseAsync(IProcess process)
        {
            return this.FileService.OpenAsync(this.Extensions, this.DefaultPickerLocation, process);
        }

        /// <summary>
        /// Load the database in the specified file.
        /// </summary>
        /// <param name="file">The file to load the database from.</param>
        /// <param name="process">The process.</param>
        /// <returns>The loaded database.</returns>
        public Task<Z> LoadAsync(IStorageFile file, IProcess process)
        {
            return this.FileService.LoadAsync<Z>(file, process);
        }

        /// <summary>
        /// Save the database to a file of the user's choosing.
        /// </summary>
        /// <param name="database">The database to save.</param>
        /// <param name="fileName">The name of the database file to save.</param>
        /// <param name="process">The process.</param>
        /// <returns>When complete.</returns>
        public Task SaveAsync(T database, string fileName, IProcess process)
        {
            return this.FileService.SaveAsync(database, fileName, this.DefaultPickerLocation, process);
        }

        #endregion
    }
}