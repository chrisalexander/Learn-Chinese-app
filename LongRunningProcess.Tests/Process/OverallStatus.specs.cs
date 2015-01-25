using Machine.Fakes;
using Machine.Specifications;
using Rhino.Mocks;
using System.Threading;

namespace LongRunningProcess.Tests.Process
{
    [Subject(typeof(LongRunningProcess.Process))]
    public class When_there_are_no_children
    {
        Establish context = () => Process = new LongRunningProcess.Process(Status, null);

        It Should_have_its_own_status = () => string.Join(string.Empty, Process.OverallStatus).ShouldEqual(Status);

        static IProcess Process;

        static string Status = "Status";
    }

    [Subject(typeof(LongRunningProcess.Process))]
    public class When_status_is_set
    {
        Establish context = () => Process = new LongRunningProcess.Process(string.Empty, null);

        Because of = () => Process.Status = Status;

        It Should_have_its_own_status = () => string.Join(string.Empty, Process.OverallStatus).ShouldEqual(Status);

        static IProcess Process;

        static string Status = "Status";
    }

    [Subject(typeof(LongRunningProcess.Process))]
    public class When_there_is_one_child : WithFakes
    {
        Establish context = () =>
            {
                Factory = An<IProcessFactory>();

                Process = new LongRunningProcess.Process(Status, Factory);

                Child = An<LongRunningProcess.IProcess>();
                Child.WhenToldTo(c => c.OverallStatus).Return(new[] { ChildStatus });
                Child.WhenToldTo(c => c.Completed).Return(false);

                Factory.WhenToldTo(c => c.Create(Param<string>.IsAnything, Param<CancellationTokenSource>.IsAnything)).Return(Child);
            };

        Because of = () => Process.Step(string.Empty, 0);

        It Should_have_both_statuses = () => string.Join(string.Empty, Process.OverallStatus).ShouldEqual(Status + ChildStatus);

        static IProcessFactory Factory;

        static IProcess Process;

        static IProcess Child;

        static string Status = "Status";

        static string ChildStatus = "Child";
    }

    [Subject(typeof(LongRunningProcess.Process))]
    public class When_there_is_one_running_child : WithFakes
    {
        Establish context = () =>
        {
            Factory = MockRepository.GenerateStub<IProcessFactory>();

            Process = new LongRunningProcess.Process(Status, Factory);

            Child = An<LongRunningProcess.IProcess>();
            Child.WhenToldTo(c => c.OverallStatus).Return(new[] { ChildStatus });
            Child.WhenToldTo(c => c.Completed).Return(true);

            Child2 = An<LongRunningProcess.IProcess>();
            Child2.WhenToldTo(c => c.OverallStatus).Return(new[] { ChildStatus2 });
            Child2.WhenToldTo(c => c.Completed).Return(false);

            Factory.Stub(f => f.Create(Arg<string>.Is.Anything, Arg<CancellationTokenSource>.Is.Anything)).Return(Child).Repeat.Once();
            Factory.Stub(f => f.Create(Arg<string>.Is.Anything, Arg<CancellationTokenSource>.Is.Anything)).Return(Child2).Repeat.Once();
        };

        Because of = () =>
        {
            Process.Step(string.Empty, 0);
            Process.Step(string.Empty, 0);
        };

        It Should_have_both_statuses = () => string.Join(string.Empty, Process.OverallStatus).ShouldEqual(Status + ChildStatus2);

        static IProcessFactory Factory;

        static IProcess Process;

        static IProcess Child;
        
        static IProcess Child2;

        static string Status = "Status";

        static string ChildStatus = "Child";

        static string ChildStatus2 = "Child2";
    }
}
