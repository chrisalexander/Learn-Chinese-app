using Machine.Specifications;

namespace KeyedItemCollection.Tests.KeyedItemCollection
{
    [Subject(typeof(KeyedItemCollection<TestItem, string>))]
    public class When_checking_contains
    {
        Establish Context = () =>
        {
            Collection = new KeyedItemCollection<TestItem, string>();
            Collection.Add(new TestItem("key1", "value1"));
            Collection.Add(new TestItem("key2", "value2"));
            Collection.Add(new TestItem("key3", "value3"));
        };

        It Should_contain_expected_item = () => Collection.Contains(new TestItem("key1", "value1")).ShouldBeTrue();

        It Should_not_contain_unexpected_item = () => Collection.Contains(new TestItem("key", "value")).ShouldBeFalse();

        static KeyedItemCollection<TestItem, string> Collection;
    }

    [Subject(typeof(KeyedItemCollection<TestItem, string>))]
    public class When_checking_contains_by_key
    {
        Establish Context = () =>
        {
            Collection = new KeyedItemCollection<TestItem, string>();
            Collection.Add(new TestItem("key1", "value1"));
            Collection.Add(new TestItem("key2", "value2"));
            Collection.Add(new TestItem("key3", "value3"));
        };

        It Should_contain_expected_item = () => Collection.ContainsKey("key1").ShouldBeTrue();

        It Should_not_contain_unexpected_item = () => Collection.ContainsKey("key").ShouldBeFalse();

        static KeyedItemCollection<TestItem, string> Collection;
    }
}
