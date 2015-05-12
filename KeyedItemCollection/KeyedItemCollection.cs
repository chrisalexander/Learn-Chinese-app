using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace KeyedItemCollection
{
    /// <summary>
    /// Keyed item collection backed by a dictionary.
    /// </summary>
    /// <typeparam name="T">The type of the object in the collection.</typeparam>
    /// <typeparam name="TZ">The type of the key of the object.</typeparam>
    [JsonObject]
    public class KeyedItemCollection<T, TZ> : IKeyedItemCollection<T, TZ> where T : IKeyedItem<TZ>
    {
        /// <summary>
        /// The backing dictionary for the collection.
        /// </summary>
        private IDictionary<TZ, T> backingDictionary;

        /// <summary>
        /// Create a new keyed item collection.
        /// </summary>
        public KeyedItemCollection()
        {
            this.backingDictionary = new Dictionary<TZ, T>();
        }

        /// <summary>
        /// Enumerate the values in the collection.
        /// </summary>
        /// <returns>The items in the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.backingDictionary.Select(pair => pair.Value).GetEnumerator();
        }

        /// <summary>
        /// Enumerate the values in the collection.
        /// </summary>
        /// <returns>The items in the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Add an item to the collection.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(T item)
        {
            if (this.backingDictionary.ContainsKey(item.Key))
            {
                this.backingDictionary[item.Key] = item;
            }
            else
            {
                this.backingDictionary.Add(item.Key, item);
            }
        }

        /// <summary>
        /// Clear the collection.
        /// </summary>
        public void Clear()
        {
            this.backingDictionary.Clear();
        }

        /// <summary>
        /// Get whether an item exists in the collection.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>Whether it exists in the collection.</returns>
        public bool Contains(T item)
        {
            return this.backingDictionary.ContainsKey(item.Key);
        }

        /// <summary>
        /// Remove an item from the collection.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>Whether it was removed.</returns>
        public bool Remove(T item)
        {
            return this.backingDictionary.Remove(item.Key);
        }

        /// <summary>
        /// Get the number of items in the collection.
        /// </summary>
        public int Count
        {
            get
            {
                return this.backingDictionary.Count;
            }
        }

        /// <summary>
        /// Get whether a key exists in the collection.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Whether it exists.</returns>
        public bool ContainsKey(TZ key)
        {
            return this.backingDictionary.ContainsKey(key);
        }

        /// <summary>
        /// Remove an item by key from the collection.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Whether the item was removed.</returns>
        public bool Remove(TZ key)
        {
            return this.backingDictionary.Remove(key);
        }

        /// <summary>
        /// Try and get an item from the collection.
        /// </summary>
        /// <param name="key">The key of the item.</param>
        /// <param name="value">The item retrieved.</param>
        /// <returns>Whether an item was retrieved.</returns>
        public bool TryGetValue(TZ key, out T value)
        {
            return this.backingDictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Indexer for the collection.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The item.</returns>
        public T this[TZ key]
        {
            get
            {
                return this.backingDictionary[key];
            }

            set
            {
                this.backingDictionary[key] = value;
            }
        }

        /// <summary>
        /// Get the keys in the collection.
        /// </summary>
        [JsonIgnore]
        public ICollection<TZ> Keys
        {
            get
            {
                return this.backingDictionary.Keys;
            }
        }

        /// <summary>
        /// Get the items in the collection.
        /// </summary>
        [JsonIgnore]
        public ICollection<T> Values
        {
            get
            {
                return this.backingDictionary.Values;
            }
        }

        /// <summary>
        /// The data that backs the collection.
        /// </summary>
        public IDictionary<TZ, T> Data
        {
            get
            {
                return this.backingDictionary;
            }

            set
            {
                this.backingDictionary = value;
            }
        }
    }
}
