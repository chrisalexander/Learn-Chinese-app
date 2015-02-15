using CourseDB.Model;
using DBUtils.Services;

namespace CourseDB.Services
{
    /// <summary>
    /// Inteface for course database manipulation services.
    /// </summary>
    public interface ICourseDatabaseService : IFileServiceWrapper<CourseDatabase, ICourseDatabase>
    {
    }
}
