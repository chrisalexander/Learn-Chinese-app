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

        It Should_have_its_own_status = () => Process.CurrentState.ShouldEqual(Status);

        static LongRunningProcess.IProcess Process;

        static string Status = "Status";
    }

    [Subject(typeof(LongRunningProcess.Process))]
    public class When_there_is_one_child : WithFakes
    {
        Establish context = () =>
            {
                Factory = An<LongRunningProcess.IProcessFactory>();

                Process = new LongRunningProcess.Process(Status, Factory);

                Child = An<LongRunningProcess.IProcess>();
                Child.WhenToldTo(c => c.CurrentState).Return(ChildStatus);
                Child.WhenToldTo(c => c.Completed).Return(false);

                Factory.WhenToldTo(c => c.Create(Param<string>.IsAnything, Param<CancellationTokenSource>.IsAnything)).Return(Child);
            };

        Because of = () => Process.Step(string.Empty, 0);

        It Should_have_both_statuses = () => Process.CurrentState.ShouldEqual(Status + "; " + ChildStatus);

        static LongRunningProcess.IProcessFactory Factory;

        static LongRunningProcess.IProcess Process;

        static LongRunningProcess.IProcess Child;

        static string Status = "Status";

        static string ChildStatus = "Child";
    }

    [Subject(typeof(LongRunningProcess.Process))]
    public class When_there_is_one_running_child : WithFakes
    {
        Establish context = () =>
        {
            Factory = MockRepository.GenerateStub<LongRunningProcess.IProcessFactory>();

            Process = new LongRunningProcess.Process(Status, Factory);

            Child = An<LongRunningProcess.IProcess>();
            Child.WhenToldTo(c => c.CurrentState).Return(ChildStatus);
            Child.WhenToldTo(c => c.Completed).Return(true);

            Child2 = An<LongRunningProcess.IProcess>();
            Child2.WhenToldTo(c => c.CurrentState).Return(ChildStatus2);
            Child2.WhenToldTo(c => c.Completed).Return(false);

            Factory.Stub(f => f.Create(Arg<string>.Is.Anything, Arg<CancellationTokenSource>.Is.Anything)).Return(Child).Repeat.Once();
            Factory.Stub(f => f.Create(Arg<string>.Is.Anything, Arg<CancellationTokenSource>.Is.Anything)).Return(Child2).Repeat.Once();
        };

        Because of = () =>
        {
            Process.Step(string.Empty, 0);
            Process.Step(string.Empty, 0);
        };

        It Should_have_both_statuses = () => Process.CurrentState.ShouldEqual(Status + "; " + ChildStatus2);

        static LongRunningProcess.IProcessFactory Factory;

        static LongRunningProcess.IProcess Process;

        static LongRunningProcess.IProcess Child;
        
        static LongRunningProcess.IProcess Child2;

        static string Status = "Status";

        static string ChildStatus = "Child";

        static string ChildStatus2 = "Child2";
    }
}
