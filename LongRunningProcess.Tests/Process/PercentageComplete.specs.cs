using Machine.Fakes;
using Machine.Specifications;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LongRunningProcess.Tests.Process
{
    [Subject(typeof(LongRunningProcess.Process))]
    public class When_marked_complete
    {
        Establish context = () => Process = new LongRunningProcess.Process(string.Empty, null);

        Because of = () => Process.Complete();

        It Should_be_at_100_percent = () => Process.PercentageComplete.ShouldEqual(100);

        static LongRunningProcess.IProcess Process;
    }

    [Subject(typeof(LongRunningProcess.Process))]
    public class When_incremented
    {
        Establish context = () => Process = new LongRunningProcess.Process(string.Empty, null);

        Because of = () => Process.Increment(50);

        It Should_be_at_50_percent = () => Process.PercentageComplete.ShouldEqual(50);

        static LongRunningProcess.IProcess Process;
    }

    [Subject(typeof(LongRunningProcess.Process))]
    public class When_incremented_over_100
    {
        Establish context = () => Process = new LongRunningProcess.Process(string.Empty, null);

        Because of = () => Process.Increment(101);

        It Should_be_capped_at_100_percent = () => Process.PercentageComplete.ShouldEqual(100);

        static LongRunningProcess.IProcess Process;
    }

    [Subject(typeof(LongRunningProcess.Process))]
    public class When_there_is_one_child_with_progress : WithFakes
    {
        Establish context = () =>
        {
            Factory = An<LongRunningProcess.IProcessFactory>();

            Process = new LongRunningProcess.Process(string.Empty, Factory);

            Child = An<LongRunningProcess.IProcess>();
            Child.WhenToldTo(c => c.PercentageComplete).Return(50);

            Factory.WhenToldTo(c => c.Create(Param<string>.IsAnything, Param<CancellationTokenSource>.IsAnything)).Return(Child);
        };

        Because of = () => Process.Step(string.Empty, 50);

        It Should_compute_the_correct_percentage = () => Process.PercentageComplete.ShouldEqual(25);

        static LongRunningProcess.IProcessFactory Factory;

        static LongRunningProcess.IProcess Process;

        static LongRunningProcess.IProcess Child;
    }

    [Subject(typeof(LongRunningProcess.Process))]
    public class When_there_are_multiple_children : WithFakes
    {
        Establish context = () =>
        {
            Factory = MockRepository.GenerateStub<LongRunningProcess.IProcessFactory>();

            Process = new LongRunningProcess.Process(string.Empty, Factory);

            Child = An<LongRunningProcess.IProcess>();
            Child.WhenToldTo(c => c.PercentageComplete).Return(10);

            Child2 = An<LongRunningProcess.IProcess>();
            Child2.WhenToldTo(c => c.PercentageComplete).Return(20);

            Factory.Stub(f => f.Create(Arg<string>.Is.Anything, Arg<CancellationTokenSource>.Is.Anything)).Return(Child).Repeat.Once();
            Factory.Stub(f => f.Create(Arg<string>.Is.Anything, Arg<CancellationTokenSource>.Is.Anything)).Return(Child2).Repeat.Once();
        };

        Because of = () =>
        {
            Process.Step(string.Empty, 40);
            Process.Step(string.Empty, 50);
        };

        It Should_combine_two_child_percentages = () => Process.PercentageComplete.ShouldEqual(14);

        static LongRunningProcess.IProcessFactory Factory;

        static LongRunningProcess.IProcess Process;

        static LongRunningProcess.IProcess Child;

        static LongRunningProcess.IProcess Child2;
    }

    [Subject(typeof(LongRunningProcess.Process))]
    public class When_there_are_multiple_overflow_children : WithFakes
    {
        Establish context = () =>
        {
            Factory = MockRepository.GenerateStub<LongRunningProcess.IProcessFactory>();

            Process = new LongRunningProcess.Process(string.Empty, Factory);

            Child = An<LongRunningProcess.IProcess>();
            Child.WhenToldTo(c => c.PercentageComplete).Return(99);

            Child2 = An<LongRunningProcess.IProcess>();
            Child2.WhenToldTo(c => c.PercentageComplete).Return(98);

            Factory.Stub(f => f.Create(Arg<string>.Is.Anything, Arg<CancellationTokenSource>.Is.Anything)).Return(Child).Repeat.Once();
            Factory.Stub(f => f.Create(Arg<string>.Is.Anything, Arg<CancellationTokenSource>.Is.Anything)).Return(Child2).Repeat.Once();
        };

        Because of = () =>
        {
            Process.Step(string.Empty, 60);
            Process.Step(string.Empty, 50);
        };

        It Should_cap_the_percentage_complete = () => Process.PercentageComplete.ShouldEqual(100);

        static LongRunningProcess.IProcessFactory Factory;

        static LongRunningProcess.IProcess Process;

        static LongRunningProcess.IProcess Child;

        static LongRunningProcess.IProcess Child2;
    }

    [Subject(typeof(LongRunningProcess.Process))]
    public class When_there_are_multiple_children_and_increment : WithFakes
    {
        Establish context = () =>
        {
            Factory = MockRepository.GenerateStub<LongRunningProcess.IProcessFactory>();

            Process = new LongRunningProcess.Process(string.Empty, Factory);

            Child = An<LongRunningProcess.IProcess>();
            Child.WhenToldTo(c => c.PercentageComplete).Return(10);

            Child2 = An<LongRunningProcess.IProcess>();
            Child2.WhenToldTo(c => c.PercentageComplete).Return(20);

            Factory.Stub(f => f.Create(Arg<string>.Is.Anything, Arg<CancellationTokenSource>.Is.Anything)).Return(Child).Repeat.Once();
            Factory.Stub(f => f.Create(Arg<string>.Is.Anything, Arg<CancellationTokenSource>.Is.Anything)).Return(Child2).Repeat.Once();
        };

        Because of = () =>
        {
            Process.Step(string.Empty, 40);
            Process.Step(string.Empty, 50);
            Process.Increment(50);
        };

        It Should_combine_two_child_percentages = () => Process.PercentageComplete.ShouldEqual(19);

        static LongRunningProcess.IProcessFactory Factory;

        static LongRunningProcess.IProcess Process;

        static LongRunningProcess.IProcess Child;

        static LongRunningProcess.IProcess Child2;
    }
}
