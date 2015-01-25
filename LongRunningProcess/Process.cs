﻿using Microsoft.Practices.Prism.Mvvm;
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
    public class Process : BindableBase, IProcess
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
        /// The overall statuses.
        /// </summary>
        private IEnumerable<string> overallStatus;

        /// <summary>
        /// How far complete this process on its own is.
        /// </summary>
        private double progress;

        /// <summary>
        /// The overall progress.
        /// </summary>
        private double overallProgress;

        /// <summary>
        /// Whether this process is completed.
        /// </summary>
        private bool completed;

        /// <summary>
        /// Construct a long running process.
        /// </summary>
        /// <param name="status">Describes what the process is doing.</param>
        /// <param name="processFactory">Factory for creating new processes.</param>
        /// <param name="cancellationSource">Cancellation token source.</param>
        public Process(string status, IProcessFactory processFactory, CancellationTokenSource cancellationSource = null)
        {
            this.childProcesses = new List<ChildProcess>();

            this.processFactory = processFactory;
            this.cancellationSource = cancellationSource ?? new CancellationTokenSource();

            this.Status = status;
            this.DurationType = ProcessDurationType.Indeterminate;
        }

        /// <summary>
        /// Gets or sets the status of this specific process.
        /// </summary>
        public string Status
        {
            get
            {
                return this.status;
            }

            set
            {
                this.SetProperty(ref this.status, value);
                this.UpdateOverallStatus();
            }
        }

        /// <summary>
        /// The statuses of the currently running processes, in hierarchical order.
        /// </summary>
        public IEnumerable<string> OverallStatus
        {
            get
            {
                return this.overallStatus;
            }

            private set
            {
                this.SetProperty(ref this.overallStatus, value);
            }
        }

        /// <summary>
        /// Get the completeness percentage of this process.
        /// </summary>
        public double Progress
        {
            get
            {
                return this.progress;
            }

            private set
            {
                this.SetProperty(ref this.progress, value);
                this.UpdateOverallProgress();
                this.DurationType = this.DurationType;
            }
        }

        /// <summary>
        /// Gets the overall progress of the process including subproccesses, as a percentage.
        /// </summary>
        public double OverallProgress
        {
            get
            {
                return this.overallProgress;
            }

            private set
            {
                this.SetProperty(ref this.overallProgress, value);
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
                var durationType = value;   

                // It should be indeterminate if it is 100% or more and not complete.
                // It should be indeterminate if there are more than 100% of work to do in child processes.
                if (this.Progress >= 100 && !this.Completed ||
                    this.childProcesses.Sum(c => c.Weighting) > 100)
                {
                    durationType = ProcessDurationType.Indeterminate;
                }

                this.SetProperty(ref this.durationType, durationType);
            }
        }

        /// <summary>
        /// Gets or sets whether or not this process has been completed.
        /// </summary>
        public bool Completed
        {
            get
            {
                return this.completed;
            }

            set
            {
                this.SetProperty(ref this.completed, value);
                this.UpdateOverallProgress();
                this.DurationType = this.DurationType;
            }
        }

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
                    // When a child's completeness changes, check ours too.
                    case "OverallProgress":
                        this.UpdateOverallProgress();
                        break;
                    // When a child's state string or completedness changes, so could ours.
                    case "Completed":
                    case "CurrentStatus":
                    case "OverallStatus":
                        this.UpdateOverallStatus();
                        break;
                }
            };

            var childProcess = new ChildProcess(process, weighting);

            this.childProcesses.Add(childProcess);

            this.UpdateOverallStatus();
            this.UpdateOverallProgress();
            this.DurationType = this.DurationType;

            return process;
        }

        /// <summary>
        /// Increment the completion percentage.
        /// </summary>
        /// <param name="amount">The amount to increment by.</param>
        public void Increment(double amount)
        {
            this.Progress += amount;
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

            var previousCompleted = this.Completed;
            
            this.Completed = true;
        }

        /// <summary>
        /// Helper to update the overall progress in response to changes in the contributing factors.
        /// </summary>
        private void UpdateOverallProgress()
        {
            // Shortcut if we are complete.
            if (this.Completed)
            {
                this.OverallProgress = 100;
                return;
            }

            var childTotal = 0.0;
            var childPercentage = this.childProcesses.Sum(c =>
            {
                childTotal += c.Weighting;
                return (c.Weighting / 100.0) * c.Process.OverallProgress;
            });

            // If children account for more than our total, return their percentage.
            if (childTotal >= 100)
            {
                this.OverallProgress = Math.Min(childPercentage, 100);
                return;
            }

            var deficitPercentage = 100.0 - childTotal;

            this.OverallProgress = Math.Min(childPercentage + (deficitPercentage * (this.Progress / 100.0)), 100);
        }

        /// <summary>
        /// Helper to update the overall status if it changes.
        /// </summary>
        private void UpdateOverallStatus()
        {
            var statuses = new List<string>();
            statuses.Add(this.Status);

            var runningChild = this.childProcesses.FirstOrDefault(c => !c.Process.Completed);
            if (runningChild != null)
            {
                statuses.AddRange(runningChild.Process.OverallStatus);
            }

            this.OverallStatus = statuses;
        }
    }
}
