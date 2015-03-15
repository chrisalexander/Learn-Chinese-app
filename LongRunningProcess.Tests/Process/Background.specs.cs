using System;
using Machine.Specifications;

namespace LongRunningProcess.Tests.Process
{
    /// <summary>
    /// Helper class for testing background actions.
    /// </summary>
    public class BackgroundActionTest
    {
        public static void ExerciseProgress(IProgress<double> progress)
        {
            progress.Report(0); // First change
            progress.Report(0.01);
            progress.Report(0.02);
            progress.Report(0.03);
            progress.Report(1); // Second change
            progress.Report(1.01);
            progress.Report(50); // Third change
            progress.Report(100); // Fourth change
        }
    }

    [Subject(typeof(LongRunningProcess.Process))]
    public class When_running_action_in_background : BackgroundActionTest
    {
        Establish Context = () =>
        {
            Process = new LongRunningProcess.Process("Test", null);
            Process.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName.Equals("Progress"))
                {
                    NotificationFireCount++;
                }
            };
        };

        Because Of = () => Process.RunInBackground((progress, token) => ExerciseProgress(progress));

        It Should_have_fired_the_currect_number_of_times = () => NotificationFireCount.ShouldEqual(4);

        It Should_not_be_complete = () => Process.Completed.ShouldBeFalse();
        
        protected static IProcess Process;

        protected static int NotificationFireCount;
    }

    [Subject(typeof(LongRunningProcess.Process))]
    public class When_running_function_in_background : BackgroundActionTest
    {
        Establish Context = () =>
        {
            Process = new LongRunningProcess.Process("Test", null);
            Process.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName.Equals("Progress"))
                {
                    NotificationFireCount++;
                }
            };
        };

        Because Of = () => Result = Process.RunInBackground((progress, token) =>
        {
            ExerciseProgress(progress);
            return "Test";
        }).Await();

        It Should_have_fired_the_currect_number_of_times = () => NotificationFireCount.ShouldEqual(4);

        It Should_not_be_complete = () => Process.Completed.ShouldBeFalse();

        It Should_return_the_correct_result = () => Result.ShouldEqual("Test");

        protected static IProcess Process;

        protected static int NotificationFireCount;

        private static string Result;
    }

    [Subject(typeof(LongRunningProcess.Process))]
    public class When_running_completing_action_in_background : BackgroundActionTest
    {
        Establish Context = () =>
        {
            Process = new LongRunningProcess.Process("Test", null);
            Process.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName.Equals("Progress"))
                {
                    NotificationFireCount++;
                }
            };
        };

        Because Of = () => Process.RunInBackground((progress, token) => ExerciseProgress(progress), true);

        It Should_have_fired_the_currect_number_of_times = () => NotificationFireCount.ShouldEqual(4);

        It Should_be_complete = () => Process.Completed.ShouldBeTrue();

        protected static IProcess Process;

        protected static int NotificationFireCount;
    }

    [Subject(typeof(LongRunningProcess.Process))]
    public class When_running_completing_function_in_background : BackgroundActionTest
    {
        Establish Context = () =>
        {
            Process = new LongRunningProcess.Process("Test", null);
            Process.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName.Equals("Progress"))
                {
                    NotificationFireCount++;
                }
            };
        };

        Because Of = () => Result = Process.RunInBackground((progress, token) =>
        {
            ExerciseProgress(progress);
            return "Test";
        }, true).Await();

        It Should_have_fired_the_currect_number_of_times = () => NotificationFireCount.ShouldEqual(4);

        It Should_be_complete = () => Process.Completed.ShouldBeTrue();

        It Should_return_the_correct_result = () => Result.ShouldEqual("Test");

        protected static IProcess Process;

        protected static int NotificationFireCount;

        private static string Result;
    }
}
