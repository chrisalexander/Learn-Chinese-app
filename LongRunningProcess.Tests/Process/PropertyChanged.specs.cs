using Machine.Fakes;
using Machine.Specifications;
using Rhino.Mocks;
using System.ComponentModel;
using System.Threading;

namespace LongRunningProcess.Tests.Process
{
    [Subject(typeof(LongRunningProcess.Process))]
    public class When_child_changes_percentage : WithFakes
    {
        Establish Context = () =>
        {
            Factory = An<IProcessFactory>();

            Process = new LongRunningProcess.Process(string.Empty, Factory);
            Process.PropertyChanged += (sender, args) => PropertyChanged = PropertyChanged || args.PropertyName.Equals("OverallProgress");

            Child = MockRepository.GenerateStub<IProcess>();
            Child.WhenToldTo(c => c.OverallStatus).Return(new[] { string.Empty });

            Factory.WhenToldTo(c => c.Create(Param<string>.IsAnything, Param<CancellationTokenSource>.IsAnything)).Return(Child);

            Process.Step(string.Empty, 10);

            Child.WhenToldTo(c => c.OverallProgress).Return(10);
        };

        Because Of = () => Child.Raise(c => c.PropertyChanged += null, null, new PropertyChangedEventArgs("OverallProgress"));

        It Should_have_notified_percentage_changed = () => PropertyChanged.ShouldBeTrue();

        static IProcessFactory Factory;

        static IProcess Process;

        static IProcess Child;

        static bool PropertyChanged;
    }

    [Subject(typeof(LongRunningProcess.Process))]
    public class When_child_completes : WithFakes
    {
        Establish Context = () =>
        {
            Factory = An<IProcessFactory>();

            Process = new LongRunningProcess.Process(string.Empty, Factory);
            Process.PropertyChanged += (sender, args) => PropertyChanged = PropertyChanged || args.PropertyName.Equals("OverallStatus");

            Child = MockRepository.GenerateStub<IProcess>();
            Child.WhenToldTo(c => c.OverallStatus).Return(new[] { string.Empty });

            Factory.WhenToldTo(c => c.Create(Param<string>.IsAnything, Param<CancellationTokenSource>.IsAnything)).Return(Child);

            Process.Step(string.Empty, 0);

            Child.Completed = true;
        };

        Because Of = () => Child.Raise(c => c.PropertyChanged += null, null, new PropertyChangedEventArgs("Completed"));

        It Should_have_notified_state_changed = () => PropertyChanged.ShouldBeTrue();

        static IProcessFactory Factory;

        static IProcess Process;

        static IProcess Child;

        static bool PropertyChanged;
    }

    [Subject(typeof(LongRunningProcess.Process))]
    public class When_child_changes_state : WithFakes
    {
        Establish Context = () =>
        {
            Factory = An<IProcessFactory>();

            Process = new LongRunningProcess.Process(string.Empty, Factory);
            Process.PropertyChanged += (sender, args) => PropertyChanged = PropertyChanged || args.PropertyName.Equals("OverallStatus");

            Child = MockRepository.GenerateStub<IProcess>();
            Child.WhenToldTo(c => c.OverallStatus).Return(new[] { string.Empty });

            Factory.WhenToldTo(c => c.Create(Param<string>.IsAnything, Param<CancellationTokenSource>.IsAnything)).Return(Child);

            Process.Step(string.Empty, 0);
        };

        Because Of = () => Child.Raise(c => c.PropertyChanged += null, null, new PropertyChangedEventArgs("Status"));

        It Should_have_notified_state_changed = () => PropertyChanged.ShouldBeTrue();

        static IProcessFactory Factory;

        static IProcess Process;

        static IProcess Child;

        static bool PropertyChanged;
    }
}
