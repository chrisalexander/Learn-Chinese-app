
namespace LongRunningProcess
{
    /// <summary>
    /// Represents a child process, which is a process and metadata
    /// about its relationship with its parent.
    /// </summary>
    public class ChildProcess
    {
        /// <summary>
        /// Creates a new child process.
        /// </summary>
        /// <param name="process">The process itself.</param>
        /// <param name="weighting">The child's weighting.</param>
        public ChildProcess(IProcess process, double weighting)
        {
            this.Process = process;
            this.Weighting = weighting;
        }

        /// <summary>
        /// The child process itself.
        /// </summary>
        public IProcess Process { get; private set; }
        
        /// <summary>
        /// The weighting of the child process in relation to its parent.
        /// </summary>
        public double Weighting { get; private set; }
    }
}
