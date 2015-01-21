using System.Threading;

namespace LongRunningProcess
{
    /// <summary>
    /// Interface for process factories.
    /// </summary>
    public interface IProcessFactory
    {
        /// <summary>
        /// Create a new IProcess implementation instance.
        /// </summary>
        /// <param name="status">The new process' status</param>
        /// <param name="cancellationSource">An optional cancellation token source for the new process.</param>
        /// <returns>The new instance.</returns>
        IProcess Create(string status, CancellationTokenSource cancellationSource = null);
    }
}
