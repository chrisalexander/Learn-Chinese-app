using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;

namespace KeyedItemCollection.Tests.KeyedItemCollection
{
    [Subject(typeof(KeyedItemCollection<TestItem, string>))]
    public class When_getting_values
    {
        Establish Context = () =>
        {
            Collection = new KeyedItemCollection<TestItem, string>();
            Collection.Add(new TestItem("key1", "value1"));
            Collection.Add(new TestItem("key2", "value2"));
            Collection.Add(new TestItem("key3", "value3"));
        };

        Because Of = () => Values = Collection.Values;

        It Should_contain_three_values = () => Values.Count.ShouldEqual(3);

        It Should_contain_first_value = () => Values.First().Value.ShouldEqual("value1");

        It Should_contain_last_value = () => Values.Last().Value.ShouldEqual("value3");

        static KeyedItemCollection<TestItem, string> Collection;

        static ICollection<TestItem> Values;
    }
}
