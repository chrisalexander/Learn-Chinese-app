using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace LangDB
{
    /// <summary>
    /// JSON implementation of the language database file service.
    /// </summary>
    [Export(typeof(ILanguageDatabaseFileService))]
    public class JsonDatabaseFileService : ILanguageDatabaseFileService
    {
        /// <summary>
        /// Maintain a local cached serializer.
        /// </summary>
        private JsonSerializer serializer;

        /// <summary>
        /// Load from JSON file.
        /// </summary>
        /// <param name="file">The file to load the database from.</param>
        /// <returns>The loaded database.</returns>
        public async Task<LanguageDatabase> LoadAsync(StorageFile file)
        {
            using (var stream = await file.OpenStreamForReadAsync())
            using (var streamReader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                var serializer = this.GetSerializer();
                return await Task.Run(() => serializer.Deserialize<LanguageDatabase>(jsonReader));
            }
        }

        /// <summary>
        /// Save JSON to a file.
        /// </summary>
        /// <param name="database">The database to save.</param>
        /// <param name="file">The file to save it to.</param>
        /// <returns>When complete.</returns>
        public async Task SaveAsync(LanguageDatabase database, StorageFile file)
        {
            using (var stream = await file.OpenStreamForWriteAsync())
            using (var streamWriter = new StreamWriter(stream))
            using (var jsonWriter = new JsonTextWriter(streamWriter))
            {
                this.ConfigureWriter(jsonWriter);

                var serializer = this.GetSerializer();
                await Task.Run(() => serializer.Serialize(jsonWriter, database));
            }
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
