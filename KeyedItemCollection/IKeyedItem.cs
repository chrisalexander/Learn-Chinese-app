namespace KeyedItemCollection
{
    /// <summary>
    /// Interface for objects which can be stored in a Keyed Item
    /// Collection, providing a method if identifying their key.
    /// </summary>
    /// <typeparam name="T">The type of the key.</typeparam>
    public interface IKeyedItem<out T>
    {
        /// <summary>
        /// Get the key of the item.
        /// </summary>
        T Key { get; }
    }
}
