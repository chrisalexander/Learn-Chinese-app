using System;
using DBUtils.Model;
using Machine.Specifications;

namespace DBUtils.Tests.Model.IDs
{
    [Subject(typeof(AbstractId))]
    public class When_getting_string_of_id
    {
        Establish Context = () =>
        {
            Guid = Guid.NewGuid();
            Id = new TestAbstractId(Guid);
        };

        It Should_have_the_same_string_as_its_id = () => Id.ToString().ShouldEqual(Guid.ToString());

        static Guid Guid;

        static AbstractId Id;
    }

    [Subject(typeof(DescendantAbstractId<>))]
    public class When_getting_string_of_descendant_with_no_parent
    {
        Establish Context = () =>
        {
            Guid = Guid.NewGuid();
            Id = new TestDescendantAbstractId(null, Guid);
        };

        It Should_have_the_same_string_as_its_id = () => Id.ToString().ShouldEqual(Guid.ToString());

        static Guid Guid;

        static AbstractId Id;
    }

    [Subject(typeof(DescendantAbstractId<>))]
    public class When_getting_string_of_descendant_with_parent
    {
        Establish Context = () =>
        {
            ParentGuid = Guid.NewGuid();
            Guid = Guid.NewGuid();
            Id = new TestDescendantAbstractId(new TestAbstractId(ParentGuid), Guid);
        };

        It Should_have_the_same_string_as_its_id = () => Id.ToString().ShouldEqual(Guid + "_" + ParentGuid);

        static Guid ParentGuid;

        static Guid Guid;

        static AbstractId Id;
    }
}
