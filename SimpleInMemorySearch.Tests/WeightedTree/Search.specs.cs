using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;

namespace SimpleInMemorySearch.Tests.WeightedTree
{
    [Subject(typeof(WeightedTree<string, char>))]
    public class When_searching_matches
    {
        Establish Context = () =>
        {
            Tree = new WeightedTree<string, char>();
            Tree.Index("test".ToCharArray(), "test", 10);
            Tree.Index("testtest".ToCharArray(), "test");
            Tree.Index("testtwo".ToCharArray(), "testtwo", 5);
            Tree.Index("twotest".ToCharArray(), "twotest", 50);
        };

        Because Of = () => Result = Tree.Search(new[] { 't' }).ToDictionary(r =>
        {
            var s = new StringBuilder();
            foreach (var c in r.Match)
            {
                s.Append(c);
            }

            return s.ToString();
        });

        It Should_have_four_results = () => Result.Count.ShouldEqual(4);

        It Should_have_first_result = () => Result.ContainsKey("test").ShouldBeTrue();

        It Should_have_first_result_value = () => Result["test"].Result.ShouldEqual("test");

        It Should_have_first_result_weight = () => Result["test"].Score.ShouldEqual(10);

        It Should_have_first_duplicate_result = () => Result.ContainsKey("testtest").ShouldBeTrue();

        It Should_have_first_duplicate_result_value = () => Result["testtest"].Result.ShouldEqual("test");

        It Should_have_first_duplicate_result_weight = () => Result["testtest"].Score.ShouldEqual(1);

        It Should_have_second_result = () => Result.ContainsKey("testtwo").ShouldBeTrue();

        It Should_have_second_result_value = () => Result["testtwo"].Result.ShouldEqual("testtwo");

        It Should_have_second_result_weight = () => Result["testtwo"].Score.ShouldEqual(5);

        It Should_have_third_result = () => Result.ContainsKey("twotest").ShouldBeTrue();

        It Should_have_third_result_value = () => Result["twotest"].Result.ShouldEqual("twotest");

        It Should_have_third_result_weight = () => Result["twotest"].Score.ShouldEqual(50);

        static IWeightedTree<string, char> Tree;

        static IDictionary<string, ITreeResult<string, char>> Result;
    }
}
