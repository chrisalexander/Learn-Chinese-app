using Machine.Specifications;

namespace KeyedItemCollection.Tests.KeyedItemCollection
{
    [Subject(typeof(KeyedItemCollection<TestItem, string>))]
    public class When_clearing_items
    {
        Establish Context = () =>
        {
            Collection = new KeyedItemCollection<TestItem, string>();
            Collection.Add(new TestItem("key1", "value1"));
            Collection.Add(new TestItem("key2", "value2"));
            Collection.Add(new TestItem("key3", "value3"));
        };

        Because Of = () => Collection.Clear();

        It Should_have_no_items = () => Collection.Count.ShouldEqual(0);

        static KeyedItemCollection<TestItem, string> Collection;
    }
}
