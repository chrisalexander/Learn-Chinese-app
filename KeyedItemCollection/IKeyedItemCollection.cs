using System.Collections.Generic;

namespace KeyedItemCollection
{
    /// <summary>
    /// Interface for the keyed item collection.
    /// </summary>
    /// <typeparam name="T">The type of the object in the collection.</typeparam>
    /// <typeparam name="TZ">The type of the key of the object.</typeparam>
    public interface IKeyedItemCollection<T, TZ> : IEnumerable<T> where T : IKeyedItem<TZ>
    {
        /// <summary>
        /// Add an item to the collection.
        /// </summary>
        /// <param name="item">The item.</param>
        void Add(T item);

        /// <summary>
        /// Clear the collection.
        /// </summary>
        void Clear();

        /// <summary>
        /// Get whether an item exists in the collection.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>Whether it exists in the collection.</returns>
        bool Contains(T item);

        /// <summary>
        /// Remove an item from the collection.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>Whether it was removed.</returns>
        bool Remove(T item);

        /// <summary>
        /// Get the number of items in the collection.
        /// </summary>
        int Count { get; }
        
        /// <summary>
        /// Get the keys in the collection.
        /// </summary>
        ICollection<TZ> Keys { get; }

        /// <summary>
        /// Get the items in the collection.
        /// </summary>
        ICollection<T> Values { get; }

        /// <summary>
        /// Get whether a key exists in the collection.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Whether it exists.</returns>
        bool ContainsKey(TZ key);

        /// <summary>
        /// Remove an item by key from the collection.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Whether the item was removed.</returns>
        bool Remove(TZ key);

        /// <summary>
        /// Try and get an item from the collection.
        /// </summary>
        /// <param name="key">The key of the item.</param>
        /// <param name="value">The item retrieved.</param>
        /// <returns>Whether an item was retrieved.</returns>
        bool TryGetValue(TZ key, out T value);

        /// <summary>
        /// Indexer for the collection.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The item.</returns>
        T this[TZ key] { get; set; }
    }
}
