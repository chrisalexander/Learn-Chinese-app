using System;
using Machine.Specifications;

namespace DBUtils.Tests.IDs
{
    [Subject(typeof(AbstractId))]
    public class When_comparing_with_object
    {
        Establish Context = () => Id = new TestAbstractId(Guid.NewGuid());

        It Should_not_equal_random_object = () => Id.Equals(new object()).ShouldBeFalse();

        static TestAbstractId Id;
    }

    [Subject(typeof(AbstractId))]
    public class When_comparing_root_and_descendant
    {
        Establish Context = () =>
        {
            var guid = Guid.NewGuid();
            Descendant = new TestDescendantAbstractId(null, guid);
            Id = new TestAbstractId(guid);
        };

        It Should_be_equal = () => Descendant.Equals(Id).ShouldBeTrue();

        It Should_be_equal_the_other_way = () => Id.Equals(Descendant).ShouldBeTrue();

        static AbstractId Descendant;

        static AbstractId Id;
    }


    [Subject(typeof(DescendantAbstractId<>))]
    public class When_comparing_descendants_same
    {
        Establish Context = () =>
        {
            var parent = Guid.NewGuid();
            var root = Guid.NewGuid();

            DescendantOne = new TestDescendantAbstractId(new TestAbstractId(parent), root);
            DescendantTwo = new TestDescendantAbstractId(new TestAbstractId(parent), root);
        };

        It Should_be_equal = () => DescendantOne.Equals(DescendantTwo).ShouldBeTrue();

        It Should_be_equal_reverse = () => DescendantTwo.Equals(DescendantOne).ShouldBeTrue();

        // ReSharper disable once PossibleUnintendedReferenceComparison
        It Should_not_be_equal_sign = () => (DescendantOne == DescendantTwo).ShouldBeFalse();

        static AbstractId DescendantOne;

        static AbstractId DescendantTwo;
    }

    [Subject(typeof(DescendantAbstractId<>))]
    public class When_comparing_descendants_different_parents
    {
        Establish Context = () =>
        {
            var root = Guid.NewGuid();

            DescendantOne = new TestDescendantAbstractId(new TestAbstractId(Guid.NewGuid()), root);
            DescendantTwo = new TestDescendantAbstractId(new TestAbstractId(Guid.NewGuid()), root);
        };

        It Should_not_be_equal = () => DescendantOne.Equals(DescendantTwo).ShouldBeFalse();

        It Should_not_be_equal_reverse = () => DescendantTwo.Equals(DescendantOne).ShouldBeFalse();

        // ReSharper disable once PossibleUnintendedReferenceComparison
        It Should_not_be_equal_sign = () => (DescendantOne == DescendantTwo).ShouldBeFalse();

        static AbstractId DescendantOne;

        static AbstractId DescendantTwo;
    }

    [Subject(typeof(DescendantAbstractId<>))]
    public class When_comparing_descendants_different_roots
    {
        Establish Context = () =>
        {
            var parent = Guid.NewGuid();

            DescendantOne = new TestDescendantAbstractId(new TestAbstractId(parent), Guid.NewGuid());
            DescendantTwo = new TestDescendantAbstractId(new TestAbstractId(parent), Guid.NewGuid());
        };

        It Should_not_be_equal = () => DescendantOne.Equals(DescendantTwo).ShouldBeFalse();

        It Should_not_be_equal_reverse = () => DescendantTwo.Equals(DescendantOne).ShouldBeFalse();

        // ReSharper disable once PossibleUnintendedReferenceComparison
        It Should_not_be_equal_sign = () => (DescendantOne == DescendantTwo).ShouldBeFalse();

        static AbstractId DescendantOne;

        static AbstractId DescendantTwo;
    }

    [Subject(typeof(DescendantAbstractId<>))]
    public class When_comparing_descendants_different
    {
        Establish Context = () =>
        {
            DescendantOne = new TestDescendantAbstractId(new TestAbstractId(Guid.NewGuid()), Guid.NewGuid());
            DescendantTwo = new TestDescendantAbstractId(new TestAbstractId(Guid.NewGuid()), Guid.NewGuid());
        };

        It Should_not_be_equal = () => DescendantOne.Equals(DescendantTwo).ShouldBeFalse();

        It Should_not_be_equal_reverse = () => DescendantTwo.Equals(DescendantOne).ShouldBeFalse();

        // ReSharper disable once PossibleUnintendedReferenceComparison
        It Should_not_be_equal_sign = () => (DescendantOne == DescendantTwo).ShouldBeFalse();

        static AbstractId DescendantOne;

        static AbstractId DescendantTwo;
    }
}
