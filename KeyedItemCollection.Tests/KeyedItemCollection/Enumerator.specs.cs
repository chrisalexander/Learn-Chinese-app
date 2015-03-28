using System.Collections.Generic;
using Machine.Specifications;

namespace KeyedItemCollection.Tests.KeyedItemCollection
{
    [Subject(typeof(KeyedItemCollection<TestItem, string>))]
    public class When_getting_enumerator
    {
        Establish Context = () =>
        {
            Collection = new KeyedItemCollection<TestItem, string>();
            Collection.Add(new TestItem("key1", "value1"));
            Collection.Add(new TestItem("key2", "value2"));
            Collection.Add(new TestItem("key3", "value3"));

            Items = new List<TestItem>();
        };

        Because Of = () =>
        {
            foreach (var item in Collection)
            {
                Items.Add(item);
            }
        };

        It Should_have_three_items = () => Items.Count.ShouldEqual(3);

        It Should_have_the_first_item = () => Items[0].Value.ShouldEqual("value1");
        
        It Should_have_the_second_item = () => Items[1].Value.ShouldEqual("value2");
        
        It Should_have_the_third_item = () => Items[2].Value.ShouldEqual("value3");

        static IKeyedItemCollection<TestItem, string> Collection;
        
        static List<TestItem> Items;
    }

    [Subject(typeof(KeyedItemCollection<TestItem, string>))]
    public class When_getting_enumerator_as_enumerable
    {
        Establish Context = () =>
        {
            var collection = new KeyedItemCollection<TestItem, string>();
            collection.Add(new TestItem("key1", "value1"));
            collection.Add(new TestItem("key2", "value2"));
            collection.Add(new TestItem("key3", "value3"));

            Collection = collection;

            Items = new List<TestItem>();
        };

        Because Of = () =>
        {
            foreach (var item in Collection)
            {
                Items.Add(item);
            }
        };

        It Should_have_three_items = () => Items.Count.ShouldEqual(3);

        It Should_have_the_first_item = () => Items[0].Value.ShouldEqual("value1");

        It Should_have_the_second_item = () => Items[1].Value.ShouldEqual("value2");

        It Should_have_the_third_item = () => Items[2].Value.ShouldEqual("value3");

        static IEnumerable<TestItem> Collection;

        static List<TestItem> Items;
    }
}
