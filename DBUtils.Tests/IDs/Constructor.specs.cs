using System;
using Machine.Specifications;

namespace DBUtils.Tests.IDs
{
    [Subject(typeof(AbstractId))]
    public class When_constructing_abstract_id_with_no_arguments
    {
        Establish Context = () => Id = new TestAbstractId();

        It Should_create_a_random_guid = () => Id.RootId.ShouldMatch("^[a-z0-9]{8}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{12}");

        static AbstractId Id;
    }

    [Subject(typeof(AbstractId))]
    public class When_constructing_descendent_id_with_no_arguments
    {
        Establish Context = () => Id = new TestDescendantAbstractId();

        It Should_create_a_random_guid = () => Id.RootId.ShouldMatch("^[a-z0-9]{8}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{12}");

        It Should_not_have_a_parent = () => Id.ParentId.ShouldBeNull();

        static DescendantAbstractId<TestAbstractId> Id;
    }

    [Subject(typeof(AbstractId))]
    public class When_constructing_descendent_id_with_parent
    {
        Establish Context = () =>
        {
            Guid = Guid.NewGuid();
            Id = new TestDescendantAbstractId(new TestAbstractId(Guid));
        };

        It Should_create_a_random_guid = () => Id.RootId.ShouldMatch("^[a-z0-9]{8}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{12}");

        It Should_have_the_parent = () => Id.ParentId.ShouldEqual(new TestAbstractId(Guid));

        static DescendantAbstractId<TestAbstractId> Id;

        static Guid Guid;
    }

    [Subject(typeof(AbstractId))]
    public class When_constructing_descendent_id_with_root
    {
        Establish Context = () => Id = new TestDescendantAbstractId("root");

        It Should_have_the_root = () => Id.RootId.ShouldEqual("root");

        It Should_not_have_a_parent = () => Id.ParentId.ShouldBeNull();

        static DescendantAbstractId<TestAbstractId> Id;
    }
}
