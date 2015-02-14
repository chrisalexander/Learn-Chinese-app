using DBUtils.Model;
using LongRunningProcess;
using Newtonsoft.Json;
using System.Composition;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace DBUtils.Services
{
    /// <summary>
    /// JSON implementation of the database file service.
    /// </summary>
    /// <typeparam name="T">The type of the database.</typeparam>
    [Export(typeof(IDatabaseFileService<>))]
    public class JsonDatabaseFileService<T> : IDatabaseFileService<T> where T : IDatabase
    {
        /// <summary>
        /// Maintain a local cached serializer.
        /// </summary>
        private JsonSerializer serializer;

        /// <summary>
        /// Load from JSON file.
        /// </summary>
        /// <typeparam name="Z">The concrete type implementation of T to load in to.</typeparam>
        /// <param name="file">The file to load the database from.</param>
        /// <param name="process">The process.</param>
        /// <returns>The loaded database.</returns>
        public async Task<Z> LoadAsync<Z>(IStorageFile file, IProcess process) where Z : T
        {
            try
            {
                using (var stream = await file.OpenStreamForReadAsync())
                using (var streamReader = new StreamReader(stream))
                using (var jsonReader = new JsonTextReader(streamReader))
                {
                    var serializer = this.GetSerializer();
                    return await process.RunInBackground((progress, token) => serializer.Deserialize<Z>(jsonReader));
                }
            }
            finally
            {
                process.Completed = true;
            }
        }

        /// <summary>
        /// Save JSON to a file.
        /// </summary>
        /// <param name="database">The database to save.</param>
        /// <param name="file">The file to save it to.</param>
        /// <param name="process">The process.</param>
        /// <returns>When complete.</returns>
        public async Task SaveAsync(T database, IStorageFile file, IProcess process)
        {
            database.Path = file.Path;

            using (var stream = await file.OpenStreamForWriteAsync())
            using (var streamWriter = new StreamWriter(stream))
            using (var jsonWriter = new JsonTextWriter(streamWriter))
            {
                this.ConfigureWriter(jsonWriter);

                var serializer = this.GetSerializer();
                await process.RunInBackground((progress, token) => serializer.Serialize(jsonWriter, database));
            }

            process.Completed = true;
        }

        /// <summary>
        /// Helper to retrieve a JSON serializer.
        /// </summary>
        /// <returns>A JSON serializer.</returns>
        private JsonSerializer GetSerializer()
        {
            if (this.serializer == null)
            {
                this.serializer = new JsonSerializer();
            }

            return this.serializer;
        }

        /// <summary>
        /// Helper to consistently configure a JSON writer.
        /// </summary>
        /// <param name="writer">The JSON writer to configure.</param>
        private void ConfigureWriter(JsonTextWriter writer)
        {
            writer.Formatting = Formatting.Indented;
        }
    }
}
