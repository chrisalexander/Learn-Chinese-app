using Machine.Specifications;

namespace KeyedItemCollection.Tests.KeyedItemCollection
{
    [Subject(typeof(KeyedItemCollection<TestItem, string>))]
    public class When_removing_present_element
    {
        Establish Context = () =>
        {
            Collection = new KeyedItemCollection<TestItem, string>();
            Collection.Add(new TestItem("key1", "value1"));
            Collection.Add(new TestItem("key2", "value2"));
            Collection.Add(new TestItem("key3", "value3"));
        };

        Because Of = () => Result = Collection.Remove(new TestItem("key1", "value1"));

        It Should_have_two_items = () => Collection.Count.ShouldEqual(2);

        It Should_have_returned_true = () => Result.ShouldBeTrue();

        static KeyedItemCollection<TestItem, string> Collection;

        static bool Result;
    }

    [Subject(typeof(KeyedItemCollection<TestItem, string>))]
    public class When_removing_absent_element
    {
        Establish Context = () =>
        {
            Collection = new KeyedItemCollection<TestItem, string>();
            Collection.Add(new TestItem("key1", "value1"));
            Collection.Add(new TestItem("key2", "value2"));
            Collection.Add(new TestItem("key3", "value3"));
        };

        Because Of = () => Result = Collection.Remove(new TestItem("key", "value"));

        It Should_have_three_items = () => Collection.Count.ShouldEqual(3);

        It Should_have_returned_false = () => Result.ShouldBeFalse();

        static KeyedItemCollection<TestItem, string> Collection;

        static bool Result;
    }

    [Subject(typeof(KeyedItemCollection<TestItem, string>))]
    public class When_removing_present_element_by_key
    {
        Establish Context = () =>
        {
            Collection = new KeyedItemCollection<TestItem, string>();
            Collection.Add(new TestItem("key1", "value1"));
            Collection.Add(new TestItem("key2", "value2"));
            Collection.Add(new TestItem("key3", "value3"));
        };

        Because Of = () => Result = Collection.Remove("key1");

        It Should_have_two_items = () => Collection.Count.ShouldEqual(2);

        It Should_have_returned_true = () => Result.ShouldBeTrue();

        static KeyedItemCollection<TestItem, string> Collection;

        static bool Result;
    }

    [Subject(typeof(KeyedItemCollection<TestItem, string>))]
    public class When_removing_absent_element_by_key
    {
        Establish Context = () =>
        {
            Collection = new KeyedItemCollection<TestItem, string>();
            Collection.Add(new TestItem("key1", "value1"));
            Collection.Add(new TestItem("key2", "value2"));
            Collection.Add(new TestItem("key3", "value3"));
        };

        Because Of = () => Result = Collection.Remove("key");

        It Should_have_three_items = () => Collection.Count.ShouldEqual(3);

        It Should_have_returned_false = () => Result.ShouldBeFalse();

        static KeyedItemCollection<TestItem, string> Collection;

        static bool Result;
    }
}
