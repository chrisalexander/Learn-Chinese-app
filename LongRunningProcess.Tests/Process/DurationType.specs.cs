using Machine.Fakes;
using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LongRunningProcess.Tests.Process
{
    [Subject(typeof(LongRunningProcess.Process))]
    public class When_duration_is_default
    {
        Establish context = () => Process = new LongRunningProcess.Process(string.Empty, null);

        It Should_be_indeterminate_by_default = () => Process.DurationType.ShouldEqual(ProcessDurationType.Indeterminate);

        static IProcess Process;
    }

    [Subject(typeof(LongRunningProcess.Process))]
    public class When_duration_is_set
    {
        Establish context = () => Process = new LongRunningProcess.Process(string.Empty, null);

        Because of = () => Process.DurationType = ProcessDurationType.Determinate;

        It Should_be_determinate = () => Process.DurationType.ShouldEqual(ProcessDurationType.Determinate);

        static IProcess Process;
    }

    [Subject(typeof(LongRunningProcess.Process))]
    public class When_process_is_over_100_but_not_complete
    {
        Establish context = () =>
        {
            Process = new LongRunningProcess.Process(string.Empty, null);
            Process.DurationType = ProcessDurationType.Determinate;
        };

        Because of = () => Process.Increment(101);

        It Should_become_indeterminate = () => Process.DurationType.ShouldEqual(ProcessDurationType.Indeterminate);

        static IProcess Process;
    }

    [Subject(typeof(LongRunningProcess.Process))]
    public class When_process_is_over_100_but_complete
    {
        Establish context = () =>
        {
            Process = new LongRunningProcess.Process(string.Empty, null);
            Process.DurationType = ProcessDurationType.Determinate;
            Process.Complete();
        };

        Because of = () => Process.Increment(101);

        It Should_be_determinate = () => Process.DurationType.ShouldEqual(ProcessDurationType.Determinate);

        static IProcess Process;
    }

    [Subject(typeof(LongRunningProcess.Process))]
    public class When_children_exceed_100_weighting : WithFakes
    {
        Establish context = () =>
        {
            Factory = An<IProcessFactory>();

            Process = new LongRunningProcess.Process(string.Empty, Factory);
            Process.DurationType = ProcessDurationType.Determinate;

            Child = An<IProcess>();
            Child.WhenToldTo(c => c.PercentageComplete).Return(50);

            Factory.WhenToldTo(c => c.Create(Param<string>.IsAnything, Param<CancellationTokenSource>.IsAnything)).Return(Child);
        };

        Because of = () =>
        {
            Process.Step(string.Empty, 50);
            Process.Step(string.Empty, 51);
        };

        It Should_become_indeterminate = () => Process.DurationType.ShouldEqual(ProcessDurationType.Indeterminate);

        static IProcessFactory Factory;

        static IProcess Process;

        static IProcess Child;
    }
}
