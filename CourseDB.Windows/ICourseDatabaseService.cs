using DBUtils.Windows;

namespace CourseDB.Windows
{
    /// <summary>
    /// Inteface for course database manipulation services.
    /// </summary>
    public interface ICourseDatabaseService : IFileServiceWrapper<Course, ICourse>
    {
    }
}
