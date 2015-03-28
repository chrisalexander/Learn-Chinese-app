using Machine.Specifications;

namespace KeyedItemCollection.Tests.KeyedItemCollection
{
    [Subject(typeof(KeyedItemCollection<TestItem, string>))]
    public class When_adding_items
    {
        Establish Context = () => Collection = new KeyedItemCollection<TestItem, string>();

        Because Of = () =>
        {
            Collection.Add(new TestItem("key1", "value1"));
            Collection.Add(new TestItem("key2", "value2"));
            Collection.Add(new TestItem("key3", "value3"));
        };

        It Should_have_three_items = () => Collection.Count.ShouldEqual(3);

        It Should_have_the_first_item = () => Collection["key1"].Value.ShouldEqual("value1");

        static IKeyedItemCollection<TestItem, string> Collection;
    }

    [Subject(typeof(KeyedItemCollection<TestItem, string>))]
    public class When_adding_items_with_duplicate_keys
    {
        Establish Context = () => Collection = new KeyedItemCollection<TestItem, string>();

        Because Of = () =>
        {
            Collection.Add(new TestItem("key1", "value1"));
            Collection.Add(new TestItem("key2", "value2"));
            Collection.Add(new TestItem("key1", "value3"));
        };

        It Should_have_two_items = () => Collection.Count.ShouldEqual(2);

        It Should_have_the_first_item_updated = () => Collection["key1"].Value.ShouldEqual("value3");

        It Should_have_the_second_item_unchanged = () => Collection["key2"].Value.ShouldEqual("value2");

        static IKeyedItemCollection<TestItem, string> Collection;
    }
}
