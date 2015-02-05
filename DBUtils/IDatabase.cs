
namespace DBUtils
{
    /// <summary>
    /// Interface for database classes.
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// The path to the database.
        /// </summary>
        string Path { get; set; }
    }
}
