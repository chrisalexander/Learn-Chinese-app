using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;

namespace KeyedItemCollection.Tests.KeyedItemCollection
{
    [Subject(typeof(KeyedItemCollection<TestItem, string>))]
    public class When_getting_keys
    {
        Establish Context = () =>
        {
            Collection = new KeyedItemCollection<TestItem, string>();
            Collection.Add(new TestItem("key1", "value1"));
            Collection.Add(new TestItem("key2", "value2"));
            Collection.Add(new TestItem("key3", "value3"));
        };

        Because Of = () => Keys = Collection.Keys;

        It Should_contain_three_keys = () => Keys.Count.ShouldEqual(3);

        It Should_contain_first_key = () => Keys.First().ShouldEqual("key1");

        It Should_contain_last_key = () => Keys.Last().ShouldEqual("key3");

        static KeyedItemCollection<TestItem, string> Collection;

        static ICollection<string> Keys;
    }
}
