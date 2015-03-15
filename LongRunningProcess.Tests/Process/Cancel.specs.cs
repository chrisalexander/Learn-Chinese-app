using Machine.Fakes;
using Machine.Specifications;
using System.Threading;
using System.Threading.Tasks;

namespace LongRunningProcess.Tests.Process
{
    [Subject(typeof(LongRunningProcess.Process))]
    public class When_cancelled
    {
        Establish Context = () =>
        {
            TokenSource = new CancellationTokenSource();
            Token = TokenSource.Token;

            Process = new LongRunningProcess.Process(string.Empty, null, TokenSource);
        };

        Because Of = () => Process.CancelAsync().Await();

        It Should_cancel_the_task = () => Token.IsCancellationRequested.ShouldBeTrue();

        It Should_be_complete = () => Process.Completed.ShouldBeTrue();

        static IProcess Process;

        static CancellationTokenSource TokenSource;

        static CancellationToken Token;
    }

    [Subject(typeof(LongRunningProcess.Process))]
    public class When_cancelled_with_children : WithFakes
    {
        Establish Context = () =>
        {
            Factory = An<IProcessFactory>();

            TokenSource = new CancellationTokenSource();
            Token = TokenSource.Token;

            Process = new LongRunningProcess.Process(string.Empty, Factory, TokenSource);

            Child = An<IProcess>();
            Child.WhenToldTo(c => c.CancelAsync()).Return(Task.Delay(0));
            Child.WhenToldTo(c => c.OverallStatus).Return(new[] { string.Empty });

            Factory.WhenToldTo(c => c.Create(Param<string>.IsAnything, Param<CancellationTokenSource>.IsAnything)).Return(Child);

            Process.Step(string.Empty, 50);
        };

        Because Of = () => Process.CancelAsync().Await();

        It Should_cancel_the_task = () => Token.IsCancellationRequested.ShouldBeTrue();

        It Should_be_complete = () => Process.Completed.ShouldBeTrue();

        It Should_cancel_the_child = () => Child.WasToldTo(c => c.CancelAsync());

        static IProcessFactory Factory;

        static IProcess Process;

        static IProcess Child;

        static CancellationTokenSource TokenSource;

        static CancellationToken Token;
    }
}
