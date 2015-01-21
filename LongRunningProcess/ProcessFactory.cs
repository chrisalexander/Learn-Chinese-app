using System.Composition;
using System.Threading;

namespace LongRunningProcess
{
    /// <summary>
    /// Factory for IProcess instances.
    /// </summary>
    [Export(typeof(IProcessFactory))]
    public class ProcessFactory : IProcessFactory
    {
        /// <summary>
        /// Create a new IProcess implementation instance.
        /// </summary>
        /// <param name="status">The new process' status</param>
        /// <param name="cancellationSource">An optional cancellation token source for the new process.</param>
        /// <returns>The new instance.</returns>
        public IProcess Create(string status, CancellationTokenSource cancellationSource = null)
        {
            return new Process(status, this, cancellationSource);
        }
    }
}
