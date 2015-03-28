namespace KeyedItemCollection.Tests.KeyedItemCollection
{
    /// <summary>
    /// Object for testing the keyed item collection.
    /// </summary>
    public class TestItem : IKeyedItem<string>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public TestItem(string key, string value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// The value.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// The key.
        /// </summary>
        public string Key { get; private set; }
    }
}
