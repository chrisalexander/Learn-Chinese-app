using DBUtils.Model;
using LongRunningProcess;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;

namespace DBUtils.Services
{
    /// <summary>
    /// Provide Windows-based handling of database files.
    /// </summary>
    public abstract class WindowsDatabaseFileService<T> : IDatabaseFileService<T> where T : IDatabase
    {
        /// <summary>
        /// Create a new database file in the specified folder.
        /// </summary>
        /// <param name="folder">The folder to put the database file in.</param>
        /// <param name="fileName">The name of the file to create.</param>
        /// <param name="process">The process.</param>
        /// <returns>The created file.</returns>
        public async Task<IStorageFile> CreateAsync(IStorageFolder folder, string fileName, IProcess process)
        {
            IStorageFile existingFile;
            try
            {
                existingFile = await folder.GetFileAsync(fileName);
            }
            catch (Exception)
            {
                existingFile = null;
            }

            // If nothing already exists then create it and we're done.
            if (existingFile == null)
            {
                process.Completed = true;
                return await folder.CreateFileAsync(fileName, CreationCollisionOption.FailIfExists);
            }

            var canUse = await this.CanUseFileAsync(folder, existingFile);

            if (!canUse)
            {
                throw new InvalidOperationException("User has requested not to use a file that already exists");
            }

            process.Completed = true;

            // It exists and we can overwrite, so return it.
            return existingFile;
        }

        /// <summary>
        /// Create a database file, asking the user for the location, with the specified filename.
        /// </summary>
        /// <param name="fileName">The name of the file to create.</param>
        /// <param name="location">The default location for the picker.</param>
        /// <param name="process">The process.</param>
        /// <returns>The created storage file.</returns>
        public async Task<IStorageFile> CreateAsync(string fileName, PickerLocationId location, IProcess process)
        {
            var folder = await this.PickFolderAsync(location, process.Step("Pick folder", 50));

            var result = await this.CreateAsync(folder, fileName, process.Step("Create the file", 50));
            
            process.Completed = true;

            return result;
        }

        /// <summary>
        /// Allow the user to specify a database file to open.
        /// </summary>
        /// <typeparam name="TZ">The concrete type to load in to.</typeparam>
        /// <param name="extensions">List of supported extensions to filter on.</param>
        /// <param name="location">The default start location.</param>
        /// <param name="process">The process.</param>
        /// <returns>The loaded database.</returns>
        public async Task<TZ> OpenAsync<TZ>(IEnumerable<string> extensions, PickerLocationId location, IProcess process) where TZ : T
        {
            var file = await this.OpenAsync(extensions, location, process.Step("Pick file", 20));

            var result = await this.LoadAsync<TZ>(file, process.Step("Load the database file", 80));

            process.Completed = true;

            return result;
        }

        /// <summary>
        /// Allow the user to specify a database file to open but not parse.
        /// </summary>
        /// <param name="extensions">List of supported extensions to filter on.</param>
        /// <param name="location">The default start location.</param>
        /// <param name="process">The process.</param>
        /// <returns>The loaded database.</returns>
        public async Task<IStorageFile> OpenAsync(IEnumerable<string> extensions, PickerLocationId location, IProcess process)
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = location;
            if (extensions != null)
            {
                foreach (var extension in extensions)
                {
                    picker.FileTypeFilter.Add(extension);
                }
            }

            var file = await picker.PickSingleFileAsync();

            if (file == null)
            {
                throw new InvalidCastException("No file picked");
            }

            process.Completed = true;

            return file;
        }

        /// <summary>
        /// Load the database in the specified file.
        /// </summary>
        /// <typeparam name="TZ">The concrete type to load in to.</typeparam>
        /// <param name="file">The file to load the database from.</param>
        /// <param name="process">The process.</param>
        /// <returns>The loaded database.</returns>
        public abstract Task<TZ> LoadAsync<TZ>(IStorageFile file, IProcess process) where TZ : T;

        /// <summary>
        /// Save the specified database to a given file.
        /// </summary>
        /// <param name="database">The database to save.</param>
        /// <param name="file">The file to save it to.</param>
        /// <param name="process">The process.</param>
        /// <returns>When complete.</returns>
        public abstract Task SaveAsync(T database, IStorageFile file, IProcess process);

        /// <summary>
        /// Save the database to a file of the user's choosing.
        /// </summary>
        /// <param name="database">The database to save.</param>
        /// <param name="fileName">The name of the database file to save.</param>
        /// <param name="location">The default location for the picker.</param>
        /// <param name="process">The process.</param>
        /// <returns>When complete.</returns>
        public async Task SaveAsync(T database, string fileName, PickerLocationId location, IProcess process)
        {
            IStorageFile targetFile;

            if (string.IsNullOrWhiteSpace(fileName))
            {
                // If no filename has been specified, then pick any file to store in to.
                targetFile = await this.PickNewFileAsync(string.Empty, "Database", string.Empty, location, process.Step("Pick file", 50));
            }
            else
            {
                // If there is a filename then create the necessary file.
                targetFile = await this.CreateAsync(fileName, location, process.Step("Create file", 50));
            }

            database.Path = targetFile.Path;

            await this.SaveAsync(database, targetFile, process.Step("Save file", 50));

            process.Completed = true;
        }

        /// <summary>
        /// Delete the specified database file.
        /// </summary>
        /// <param name="file">The file to delete.</param>
        /// <param name="process">The process.</param>
        /// <returns>When complete.</returns>
        public async Task DeleteAsync(IStorageFile file, IProcess process)
        {
            await file.DeleteAsync();

            process.Completed = true;
        }

        /// <summary>
        /// Ask the user whether it is OK to overwrite the specified file in the specified directory.
        /// </summary>
        /// <param name="folder">The folder the file is in.</param>
        /// <param name="file">The file itself.</param>
        /// <returns>Whether it is ok to overwrite the file.</returns>
        private async Task<bool> CanUseFileAsync(IStorageFolder folder, IStorageFile file)
        {
            var canUse = false;

            var messageDialog = new MessageDialog("There is already a file with that name " + file.Name + " in this folder " + folder.Name + ", would you like to use it?", "Existing file found");

            messageDialog.Commands.Add(new UICommand("Use existing file", cmd =>
            {
                canUse = true;
            }));

            messageDialog.Commands.Add(new UICommand("Pick again", cmd =>
            {
                canUse = false;
            }));

            messageDialog.DefaultCommandIndex = 0;
            messageDialog.CancelCommandIndex = 1;
            await messageDialog.ShowAsync();

            return canUse;
        }

        /// <summary>
        /// Picks a new file to save to the filesystem.
        /// </summary>
        /// <param name="suggestedFileName">The recommended name of the file.</param>
        /// <param name="extensionType">The string description of the file extension.</param>
        /// <param name="extension">The file extension to save with.</param>
        /// <param name="location">The default location for the picker to go to.</param>
        /// <param name="process">The process.</param>
        /// <returns>The picked file.</returns>
        private async Task<IStorageFile> PickNewFileAsync(string suggestedFileName, string extensionType, string extension, PickerLocationId location, IProcess process)
        {
            var picker = new FileSavePicker();
            picker.FileTypeChoices.Add(extensionType, new List<string> { extension });
            picker.SuggestedStartLocation = location;
            picker.SuggestedFileName = suggestedFileName;

            var file = await picker.PickSaveFileAsync();

            if (file == null)
            {
                throw new InvalidCastException("No file picked");
            }

            process.Completed = true;

            return file;
        }

        /// <summary>
        /// Helper to pick a folder to store a file in.
        /// </summary>
        /// <param name="location">Default location for the picker to go to.</param>
        /// <param name="process">The process.</param>
        /// <returns>The selected folder.</returns>
        private async Task<IStorageFolder> PickFolderAsync(PickerLocationId location, IProcess process)
        {
            var picker = new FolderPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = location;
            picker.FileTypeFilter.Add("*");

            var folder = await picker.PickSingleFolderAsync();

            if (folder == null)
            {
                throw new InvalidOperationException("No folder selected");
            }

            process.Completed = true;

            return folder;
        }
    }
}
