using System.Collections.Generic;
using Machine.Specifications;

namespace SimpleInMemorySearch.Tests.WeightedTree
{
    [Subject(typeof(WeightedTree<string, char>))]
    public class When_indexing_with_no_term
    {
        Establish Context = () => Tree = new WeightedTree<string, char>();

        Because Of = () => Tree.Index(new List<char>(), "test");

        It Should_not_have_any_edges = () => Tree.Edges.Count.ShouldEqual(0);

        It Should_have_one_item = () => Tree.Items.Count.ShouldEqual(1);

        It Should_have_the_right_item = () => Tree.Items.ContainsKey("test").ShouldBeTrue();

        static IWeightedTree<string, char> Tree;
    }

    [Subject(typeof(WeightedTree<string, char>))]
    public class When_indexing_with_one_character_key
    {
        Establish Context = () => Tree = new WeightedTree<string, char>();

        Because Of = () => Tree.Index(new[] { 'a' }, "test");

        It Should_have_one_edge = () => Tree.Edges.Count.ShouldEqual(1);

        It Should_have_the_right_edge = () => Tree.Edges.ContainsKey('a').ShouldBeTrue();

        It Should_have_no_items = () => Tree.Items.Count.ShouldEqual(0);

        It Should_have_the_right_item = () => Tree.Edges['a'].Items.ContainsKey("test").ShouldBeTrue();

        static IWeightedTree<string, char> Tree;
    }

    [Subject(typeof(WeightedTree<string, char>))]
    public class When_indexing_with_multiple_character_key
    {
        Establish Context = () => Tree = new WeightedTree<string, char>();

        Because Of = () => Tree.Index("test".ToCharArray(), "test");

        It Should_have_one_edge = () => Tree.Edges.Count.ShouldEqual(1);

        It Should_have_the_right_edge = () => Tree.Edges.ContainsKey('t').ShouldBeTrue();

        It Should_have_no_items = () => Tree.Items.Count.ShouldEqual(0);

        It Should_have_the_right_item = () => Tree.Edges['t'].Edges['e'].Edges['s'].Edges['t'].Items.ContainsKey("test").ShouldBeTrue();

        static IWeightedTree<string, char> Tree;
    }

    [Subject(typeof(WeightedTree<string, char>))]
    public class When_indexing_with_nonstandard_weight
    {
        Establish Context = () => Tree = new WeightedTree<string, char>();

        Because Of = () => Tree.Index(new[] { 'a' }, "test", 10);

        It Should_have_one_edge = () => Tree.Edges.Count.ShouldEqual(1);

        It Should_have_the_right_edge = () => Tree.Edges.ContainsKey('a').ShouldBeTrue();

        It Should_have_no_items = () => Tree.Items.Count.ShouldEqual(0);

        It Should_have_the_right_weight = () => Tree.Weight.ShouldEqual(10);

        It Should_have_the_right_item = () => Tree.Edges['a'].Items.ContainsKey("test").ShouldBeTrue();

        It Should_have_the_right_child_weight = () => Tree.Edges['a'].Weight.ShouldEqual(10);

        static IWeightedTree<string, char> Tree;
    }

    [Subject(typeof(WeightedTree<string, char>))]
    public class When_indexing_multiple
    {
        Establish Context = () => Tree = new WeightedTree<string, char>();

        Because Of = () =>
        {
            Tree.Index("test".ToCharArray(), "test", 10);
            Tree.Index("testtwo".ToCharArray(), "testtwo", 5);
            Tree.Index("twotest".ToCharArray(), "twotest", 50);
        };

        It Should_have_one_edge = () => Tree.Edges.Count.ShouldEqual(1);

        It Should_have_the_right_edge = () => Tree.Edges.ContainsKey('t').ShouldBeTrue();

        It Should_have_no_items = () => Tree.Items.Count.ShouldEqual(0);

        It Should_have_the_right_weight = () => Tree.Weight.ShouldEqual(65);

        It Should_have_the_right_first_item = () => Tree.Edges['t'].Edges['e'].Edges['s'].Edges['t'].Items.ContainsKey("test").ShouldBeTrue();

        It Should_have_the_right_second_item = () => Tree.Edges['t'].Edges['e'].Edges['s'].Edges['t'].Edges['t'].Edges['w'].Edges['o'].Items.ContainsKey("testtwo").ShouldBeTrue();

        It Should_have_the_right_third_item = () => Tree.Edges['t'].Edges['w'].Edges['o'].Edges['t'].Edges['e'].Edges['s'].Edges['t'].Items.ContainsKey("twotest").ShouldBeTrue();

        It Should_have_the_right_first_weight = () => Tree.Edges['t'].Edges['e'].Edges['s'].Edges['t'].Weight.ShouldEqual(15);

        It Should_have_the_right_second_weight = () => Tree.Edges['t'].Edges['e'].Edges['s'].Edges['t'].Edges['t'].Edges['w'].Edges['o'].Weight.ShouldEqual(5);

        It Should_have_the_right_third_weight = () => Tree.Edges['t'].Edges['w'].Edges['o'].Edges['t'].Edges['e'].Edges['s'].Edges['t'].Weight.ShouldEqual(50);

        static IWeightedTree<string, char> Tree;
    }
}
