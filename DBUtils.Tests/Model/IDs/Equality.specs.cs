using System;
using DBUtils.Model;
using Machine.Specifications;

namespace DBUtils.Tests.Model.IDs
{
    [Subject(typeof(AbstractId))]
    public class When_comparing_root_and_descendant
    {
        Establish Context = () =>
        {
            var guid = Guid.NewGuid();
            Descendant = new TestDescendantAbstractId(null, guid);
            Id = new TestAbstractId(guid);
        };

        // ReSharper disable once SuspiciousTypeConversion.Global
        It Should_be_equal = () => Descendant.Equals(Id).ShouldBeTrue();

        // ReSharper disable once SuspiciousTypeConversion.Global
        It Should_be_equal_the_other_way = () => Id.Equals(Descendant).ShouldBeTrue();

        private static TestDescendantAbstractId Descendant;

        private static TestAbstractId Id;
    }


    public class TestAbstractId : AbstractId
    {
        public TestAbstractId(Guid root)
        {
            this.RootId = root;
        }
    }

    public class TestDescendantAbstractId : DescendantAbstractId<TestAbstractId>
    {
        public TestDescendantAbstractId(TestAbstractId parent, Guid root)
        {
            this.ParentId = parent;
            this.RootId = root;
        }
    }
}
