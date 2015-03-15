using Machine.Specifications;

namespace LongRunningProcess.Tests.ProcessFactory
{
    [Subject(typeof(LongRunningProcess.ProcessFactory))]
    public class When_creating_new_instance
    {
        Establish Context = () => Factory = new LongRunningProcess.ProcessFactory();

        Because Of = () => Process = Factory.Create("Test");

        It Should_create_with_the_correct_name = () => Process.Status.ShouldEqual("Test");

        private static IProcessFactory Factory;

        private static IProcess Process;
    }
}
