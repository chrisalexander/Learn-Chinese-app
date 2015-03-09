using System;
using System.Threading.Tasks;
using CourseDB.Model;
using DBUtils.Services;
using System.Collections.Generic;
using System.Composition;
using Windows.Storage.Pickers;
using LongRunningProcess;

namespace CourseDB.Services
{
    /// <summary>
    /// Service for manipulating course databases.
    /// </summary>
    [Export(typeof(ICourseDatabaseService))]
    public class CourseDatabaseService : AbstractFileServiceWrapper<CourseDatabase, ICourseDatabase>, ICourseDatabaseService
    {
        /// <summary>
        /// The database file service.
        /// </summary>
        private readonly IDatabaseFileService<ICourseDatabase> fileService;

        /// <summary>
        /// The supported file extensions of course databases.
        /// </summary>
        private readonly IEnumerable<string> extensions = new[] { ".coursedb" };

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="fileService">The file service.</param>
        [ImportingConstructor]
        public CourseDatabaseService(IDatabaseFileService<ICourseDatabase> fileService)
        {
            this.fileService = fileService;
        }

        /// <summary>
        /// Provide the file service to the abstract wrapper.
        /// </summary>
        protected override IDatabaseFileService<ICourseDatabase> FileService
        {
            get
            {
                return this.fileService;
            }
        }

        /// <summary>
        /// Provide the default picker location to the abstract wrapper.
        /// </summary>
        protected override PickerLocationId DefaultPickerLocation
        {
            get
            {
                return PickerLocationId.DocumentsLibrary;
            }
        }

        /// <summary>
        /// Provide the supported extensions to the abstract wrapper.
        /// </summary>
        protected override IEnumerable<string> Extensions
        {
            get
            {
                return this.extensions;
            }
        }

        /// <summary>
        /// Provide metadata regarding the database prior to save.
        /// </summary>
        /// <param name="database">The database.</param>
        /// <param name="process">The process.</param>
        /// <returns>When complete.</returns>
        protected override async Task PreSaveStep(ICourseDatabase database, IProcess process)
        {
            database.Updated = DateTime.Now;

            process.Completed = true;
        }
    }
}
