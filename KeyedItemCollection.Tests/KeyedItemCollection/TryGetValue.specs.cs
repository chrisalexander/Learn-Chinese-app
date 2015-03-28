using Machine.Specifications;

namespace KeyedItemCollection.Tests.KeyedItemCollection
{
    [Subject(typeof(KeyedItemCollection<TestItem, string>))]
    public class When_getting_value_present
    {
        Establish Context = () =>
        {
            Collection = new KeyedItemCollection<TestItem, string>();
            Collection.Add(new TestItem("key1", "value1"));
            Collection.Add(new TestItem("key2", "value2"));
            Collection.Add(new TestItem("key3", "value3"));
        };

        Because Of = () => Collection.TryGetValue("key1", out Result);

        It Should_not_return_null = () => Result.ShouldNotBeNull();

        It Should_return_the_right_result = () => Result.Value.ShouldEqual("value1");

        static KeyedItemCollection<TestItem, string> Collection;

        static TestItem Result;
    }

    [Subject(typeof(KeyedItemCollection<TestItem, string>))]
    public class When_getting_value_absent
    {
        Establish Context = () =>
        {
            Collection = new KeyedItemCollection<TestItem, string>();
            Collection.Add(new TestItem("key1", "value1"));
            Collection.Add(new TestItem("key2", "value2"));
            Collection.Add(new TestItem("key3", "value3"));
        };

        Because Of = () => Collection.TryGetValue("key", out Result);

        It Should_return_null = () => Result.ShouldBeNull();

        static KeyedItemCollection<TestItem, string> Collection;

        static TestItem Result;
    }
}
