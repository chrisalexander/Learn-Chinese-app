﻿using LongRunningProcess;
using System;
using System.Collections.Generic;
using System.Composition;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;

namespace LangDB
{
    [Export(typeof(IArchiveAcquisitionService))]
    public class ArchiveAcquisitionService : IArchiveAcquisitionService
    {
        /// <summary>
        /// Acquire a URI and return the lines within the file.
        /// </summary>
        /// <param name="uri">The URI of the archive to retrieve.</param>
        /// <param name="process">The process.</param>
        /// <returns>Each of the lines in the file.</returns>
        public async Task<IList<string>> GetLinesAsync(Uri uri, IProcess process)
        {
            var temporaryFile = await this.DownloadFile(uri, process.Step("Downloading", 45));

            var archiveFolder = await this.ExtractArchive(temporaryFile, process.Step("Extracting", 45));

            var targetFile = await archiveFolder.GetFileAsync("cedict_ts.u8");

            var lines = await FileIO.ReadLinesAsync(targetFile);

            var deleteStep = process.Step("Cleaning up", 10);
            deleteStep.DurationType = ProcessDurationType.Determinate;

            await temporaryFile.DeleteAsync();
            deleteStep.Increment(50);
            await archiveFolder.DeleteAsync();
            deleteStep.Completed = true;

            process.Completed = true;

            return lines;
        }

        /// <summary>
        /// Downloads the URI to the filesystem and returns the file that was downloaded.
        /// </summary>
        /// <param name="uri">The URI to download.</param>
        /// <returns>The file that was downloaded.</returns>
        private async Task<IStorageFile> DownloadFile(Uri uri, IProcess process)
        {
            process.DurationType = ProcessDurationType.Determinate;

            var destination = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(Guid.NewGuid().ToString());

            var downloader = new BackgroundDownloader();
            var download = downloader.CreateDownload(uri, destination);

            await download.StartAsync().AsTask(process.CancellationToken, new Progress<DownloadOperation>((operation) =>
            {
                process.Increment(operation.Progress.BytesReceived / operation.Progress.TotalBytesToReceive);
            }));

            process.Completed = true;

            return destination;
        }

        /// <summary>
        /// Extracts a file that is an archive in to a folder, returning the folder.
        /// </summary>
        /// <param name="file">The file to extract.</param>
        /// <returns>The folder the file was extracted in to.</returns>
        private async Task<IStorageFolder> ExtractArchive(IStorageFile file, IProcess process)
        {
            process.DurationType = ProcessDurationType.Determinate;

            var targetFolder = await ApplicationData.Current.TemporaryFolder.CreateFolderAsync(Guid.NewGuid().ToString());

            using (var stream = await file.OpenStreamForReadAsync())
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Read))
            {
                var stepSize = 100.0 / archive.Entries.Count;
                foreach (var entry in archive.Entries)
                {
                    var targetFile = await targetFolder.CreateFileAsync(entry.FullName, CreationCollisionOption.GenerateUniqueName);
                    using (var fileStream = await targetFile.OpenStreamForWriteAsync())
                    using (var archiveStream = entry.Open())
                    {
                        const int BUFFER_SIZE = 1024;
                        byte[] buffer = new byte[BUFFER_SIZE];

                        int bytesread = 0;
                        while ((bytesread = await archiveStream.ReadAsync(buffer, 0, BUFFER_SIZE)) > 0)
                        {
                            await fileStream.WriteAsync(buffer, 0, bytesread);
                        }

                        process.CancellationToken.ThrowIfCancellationRequested();
                    }

                    process.Increment(stepSize);
                }
            }

            process.Completed = true;

            return targetFolder;
        }
    }
}
