using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LongRunningProcess
{
    /// <summary>
    /// A long running process allows a process to report progress
    /// and steps it is taking, along with receiving cancellation
    /// notifications.
    /// </summary>
    public class Process : IProcess, INotifyPropertyChanged
    {
        /// <summary>
        /// Factory for creating new processes.
        /// </summary>
        private readonly IProcessFactory processFactory;

        /// <summary>
        /// The cancellation token source for the process.
        /// </summary>
        private readonly CancellationTokenSource cancellationSource;

        /// <summary>
        /// The child processes of this process.
        /// </summary>
        private IList<ChildProcess> childProcesses;

        /// <summary>
        /// The duration type of this process.
        /// </summary>
        private ProcessDurationType durationType;

        /// <summary>
        /// The status of this specific process.
        /// </summary>
        private string status;

        /// <summary>
        /// How far complete this process on its own is.
        /// </summary>
        private double completeness;

        /// <summary>
        /// Construct a long running process.
        /// </summary>
        /// <param name="status">Describes what the process is doing.</param>
        /// <param name="processFactory">Factory for creating new processes.</param>
        /// <param name="cancellationSource">Cancellation token source.</param>
        public Process(string status, IProcessFactory processFactory, CancellationTokenSource cancellationSource = null)
        {
            this.status = status;
            this.processFactory = processFactory;
            this.cancellationSource = cancellationSource ?? new CancellationTokenSource();
            this.childProcesses = new List<ChildProcess>();
            this.durationType = ProcessDurationType.Indeterminate;
        }

        /// <summary>
        /// Get the string describing what is currently being worked on in the process.
        /// </summary>
        public string CurrentState
        {
            get
            {
                var childStatus = this.childProcesses.FirstOrDefault(c => !c.Process.Completed);
                return this.status + (childStatus != null ? "; " + childStatus.Process.CurrentState : string.Empty);
            }
        }

        /// <summary>
        /// Get the completeness percentage of this process.
        /// </summary>
        public double PercentageComplete
        {
            get
            {
                // Shortcut if we are complete.
                if (this.Completed)
                {
                    return 100;
                }

                var childTotal = 0.0;
                var childPercentage = this.childProcesses.Sum(c =>
                {
                    childTotal += c.Weighting;
                    return (c.Weighting / 100.0) * c.Process.PercentageComplete;
                });

                // If children account for more than our total, return their percentage.
                if (childTotal >= 100)
                {
                    return Math.Min(childPercentage, 100);
                }

                var deficitPercentage = 100.0 - childTotal;

                return Math.Min(childPercentage + (deficitPercentage * (this.completeness / 100.0)), 100);
            }
        }

        /// <summary>
        /// The duration type of this process.
        /// </summary>
        public ProcessDurationType DurationType
        {
            get
            {
                return this.durationType;
            }
            set
            {
                this.durationType = value;
                this.AssessDurationType();
                this.OnPropertyChanged("DurationType");
            }
        }

        /// <summary>
        /// Return whether or not this process has been completed.
        /// </summary>
        public bool Completed { get; private set; }

        /// <summary>
        /// Get a cancellation token for the current process.
        /// </summary>
        public CancellationToken CancellationToken
        {
            get
            {
                return this.cancellationSource.Token;
            }
        }

        /// <summary>
        /// The property changed event handler.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Create a step in the current process.
        /// </summary>
        /// <param name="name">Name of the process step, user-facing.</param>
        /// <param name="weighting">Weighting as a percentage of the total process.</param>
        /// <param name="durationType">The type of the duration.</param>
        /// <returns></returns>
        public IProcess Step(string name, double weighting)
        {
            var process = this.processFactory.Create(name, this.cancellationSource);

            process.PropertyChanged += (sender, args) =>
            {
                switch (args.PropertyName)
                {
                    // When a child's completeness changes, so does ours.
                    case "PercentageComplete":
                        this.OnPropertyChanged("PercentageComplete");
                        break;
                    // When a child's state string or completedness changes, so does ours.
                    case "Completed":
                    case "CurrentState":
                        this.OnPropertyChanged("CurrentState");
                        break;
                }
            };

            var childProcess = new ChildProcess(process, weighting);

            this.childProcesses.Add(childProcess);

            this.AssessDurationType();
            this.OnPropertyChanged("PercentageComplete");

            return process;
        }

        /// <summary>
        /// Increment the completion percentage.
        /// </summary>
        /// <param name="amount">The amount to increment by.</param>
        public void Increment(double amount)
        {
            this.completeness += amount;
            this.OnPropertyChanged("PercentageComplete");
            this.AssessDurationType();
        }

        /// <summary>
        /// Mark this current process, and all child processes, as complete.
        /// </summary>
        public void Complete()
        {
            this.Completed = true;
            this.OnPropertyChanged("Completed");
            this.OnPropertyChanged("PercentageComplete");
        }

        /// <summary>
        /// Update the status describing the process.
        /// </summary>
        /// <param name="status">The new status of the process.</param>
        public void Status(string status)
        {
            this.status = status;
            this.OnPropertyChanged("CurrentState");
        }

        /// <summary>
        /// Cancels the process.
        /// </summary>
        /// <returns>When cancellation is completed.</returns>
        public async Task CancelAsync()
        {
            if (this.childProcesses.Count > 0)
            {
                var tasks = this.childProcesses.Select(c => c.Process.CancelAsync());

                await Task.WhenAll(tasks);
            }

            await Task.Run(() => this.cancellationSource.Cancel());

            this.Completed = true;
            this.OnPropertyChanged("Completed");
        }

        /// <summary>
        /// Notify any listeners that a property has changed.
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged(string propertyName)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Assess the current progress of this process and evaluate whether
        /// it should be indeterminate.
        /// </summary>
        private void AssessDurationType()
        {
            // It should be indeterminate if it is 100% or more and not complete.
            // It should be indeterminate if there are more than 100% of work to do in child processes.
            if (this.completeness >= 100 && !this.Completed || 
                this.childProcesses.Sum(c => c.Weighting) > 100)
            {
                this.durationType = ProcessDurationType.Indeterminate;
                this.OnPropertyChanged("DurationType");
            }
        }
    }
}
