using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace LongRunningProcess
{
    /// <summary>
    /// Interface for objects which are given to a long-running process, which
    /// may be formed of multiple steps and child steps, which then allow progress
    /// reporting and cancellation of that process.
    /// </summary>
    public interface IProcess
    {
        /// <summary>
        /// Get the string describing what is currently being worked on in the process.
        /// </summary>
        string CurrentState { get; }

        /// <summary>
        /// Get the completeness percentage of this process.
        /// </summary>
        double PercentageComplete { get; }

        /// <summary>
        /// The duration type of this process.
        /// </summary>
        ProcessDurationType DurationType { get; set; }

        /// <summary>
        /// Return whether or not this process has been completed.
        /// </summary>
        bool Completed { get; }

        /// <summary>
        /// Get a cancellation token for the current process.
        /// </summary>
        CancellationToken CancellationToken { get; }

        /// <summary>
        /// Event for when a property is changed.
        /// </summary>
        event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Create a step in the current process, with a defined name, weighting as part
        /// of the total process, and a type of duration.
        /// The defined name for the step should be renderable to a user, so it should be
        /// a succinct and accurate representation of what is going on.
        /// The weighting is, out of 100, how much the step will contribute to the overall
        /// progress of the current process. Note that if the total estimated percentage
        /// goes over 100, the implementation shall become indeterminate.
        /// Returns an IProcess which is effectively a child of the current process.
        /// </summary>
        /// <param name="name">Name of the process step, user-facing.</param>
        /// <param name="weighting">Weighting as a percentage of the total process.</param>
        /// <param name="durationType">The type of the duration.</param>
        /// <returns></returns>
        IProcess Step(string name, double weighting);

        /// <summary>
        /// Increment the completion percentage of the curret process by the specified amount.
        /// Note if the total goes over 100 without being flagged as complete, the process
        /// will become of duration type Indeterminate.
        /// </summary>
        /// <param name="amount">The amount to increment by.</param>
        void Increment(double amount);

        /// <summary>
        /// Mark this current process, and all child processes, as complete.
        /// </summary>
        void Complete();

        /// <summary>
        /// Update the status describing the process.
        /// </summary>
        /// <param name="status">The new status of the process.</param>
        void Status(string status);

        /// <summary>
        /// Cancels the process.
        /// </summary>
        /// <returns>When cancellation is completed.</returns>
        Task CancelAsync();
    }
}
