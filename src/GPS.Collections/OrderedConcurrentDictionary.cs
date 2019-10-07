/*
    # GPS.Collections

    ## OrderedConcurrentDictionary.cs

    Data structure that comprises an implementation of
    IDictionary<TKey, TValue> that is backed by the ConcurrentDictionary
    and ConcurrentQueue objects.

    ## Copyright

    2019 - Gateway Programming School, Inc.

    This notice must be retained for any use the code
    herein in whole or in part for any use.
 */

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GPS.Collections
{
    /// <summary>
    /// Collection of generic key-value pairs that
    /// uses a ConcurrentDictionary&lt;TKey, TValue&gt;
    /// along with a ConcurrentQueue&lt;(TKey Key, TValue Value)&gt;
    /// instances holding the values of the collection.
    /// </summary>
    /// <remarks>
    /// Accessing the data randomly is performed by pulling from the
    /// ConcurrentDictionary and acessing the data sequentially is
    /// performed by pulling from the ConcurrentQueue.
    /// </remarks>
    /// <typeparam name="TKey">Type of the Key</typeparam>
    /// <typeparam name="TValue">Type of the Value</typeparam>
    public class OrderedConcurrentDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly object _lock = new object();

        private ConcurrentQueue<(TKey Key, TValue Value)> _queue = new ConcurrentQueue<(TKey Key, TValue Value)>();

        private IDictionary<TKey, TValue> _dictionary = new ConcurrentDictionary<TKey, TValue>();

        /// <summary>
        /// Default constructor that initializes an empty collection.
        /// </summary>
        public OrderedConcurrentDictionary()
        {
            _queue = new ConcurrentQueue<(TKey Key, TValue Value)>();

            _dictionary = new ConcurrentDictionary<TKey, TValue>();
        }

        /// <summary>
        /// Constructor that initializes the collection with the supplied dictionary.
        /// </summary>
        /// <param name="data">IDictionary&lt;TKey, TValue&gt; containing the initial
        /// data for the collection.</param>
        public OrderedConcurrentDictionary(IDictionary<TKey, TValue> data)
        {
            _queue = new ConcurrentQueue<(TKey Key, TValue Value)>(data.Select(p => (p.Key, p.Value)));

            _dictionary = new ConcurrentDictionary<TKey, TValue>(data);
        }


        /// <summary>
        /// Constructor that initializes the collection with the supplied collection.
        /// </summary>
        /// <param name="data">IEnumerable&lt;(TKey, TValue)&gt; containing the initial
        /// data for the collection.</param>
        public OrderedConcurrentDictionary(IEnumerable<(TKey Key, TValue Value)> data)
        {
            _queue = new ConcurrentQueue<(TKey Key, TValue Value)>(data);

            _dictionary = new ConcurrentDictionary<TKey, TValue>(data.Select(t => KeyValuePair.Create(t.Key, t.Value)));
        }

        /// <summary>
        /// Public indexer that retrieves data directly from the
        /// underlying ConcurrentDictionary.
        /// </summary>
        /// <value>TValue value for the supplied key.</value>
        /// <exception member="System.Collections.Generic.KeyNotFoundException">
        /// Thrown if the requested Key does not exist in the collection.</exception>
        public TValue this[TKey key]
        {
            get => _dictionary[key];
            set
            {
                lock (_lock)
                {
                    _dictionary[key] = value;
                    var pair = _queue.FirstOrDefault(p => p.Key.Equals(key));
                    if (!pair.Equals(default))
                    {
                        pair.Value = value;
                    }
                }
            }
        }

        /// <summary>
        /// Keys in the collection.
        /// </summary>
        public ICollection<TKey> Keys => _queue.Select(p => p.Key).ToList();

        /// <summary>
        /// Values in the collection.
        /// </summary>
        public ICollection<TValue> Values => _queue.Select(p => p.Value).ToList();

        /// <summary>
        /// Number of items in the collection.
        /// </summary>
        public int Count => _dictionary.Count;

        /// <summary>
        /// Is the collection Reay Only?
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Add a key-value pair to the collection.
        /// </summary>
        /// <param name="key">Key of the pair.</param>
        /// <param name="value">Value of the pair.</param>
        public void Add(TKey key, TValue value)
        {
            lock (_lock)
            {
                if (!_dictionary.ContainsKey(key))
                {
                    _queue.Enqueue((key, value));
                    _dictionary.TryAdd(key, value);
                }
                else
                {
                    this[key] = value;
                }
            }
        }

        /// <summary>
        /// Adds the KeyValuePair to the collection.
        /// </summary>
        /// <param name="item">Data to add to the collection.</param>
        /// <remarks>The KeyValuePair struct is not preserved.false</remarks>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// Resets the collection to an empty state.
        /// </summary>
        public void Clear()
        {
            lock (_lock)
            {
                _dictionary = new ConcurrentDictionary<TKey, TValue>();
                _queue = new ConcurrentQueue<(TKey Key, TValue Value)>();
            }
        }

        /// <summary>
        /// Tests if the collection contains the specified 
        /// KeyValuePair struct.
        /// </summary>
        /// <param name="item">Data to search for.</param>
        /// <returns>True if the data is found in the collection.</returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _dictionary.Contains(item);
        }

        /// <summary>
        /// Tests if the key is found in the collection.
        /// </summary>
        /// <param name="key">Key to search for.</param>
        /// <returns>True if the key is found in the collection.</returns>
        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Copies the data of the collection from the ConcurrentQueue
        /// into the provided array beginning at the specified index.
        /// </summary>
        /// <param name="array">Target array.</param>
        /// <param name="arrayIndex">Beginning index to copy to.</param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            lock (_lock)
            {
                var arr = new (TKey Key, TValue Value)[_queue.Count];
                _queue.CopyTo(arr, arrayIndex);

                var pairs = arr.Select(t => KeyValuePair.Create(t.Key, t.Value)).ToList();
                pairs.CopyTo(array, arrayIndex);
            }
        }

        /// <summary>
        /// Get an instance of an IEnumerator&lt;KeyValuePair(TKey, TValue)&gt;
        /// for the data in the collection.
        /// </summary>
        /// <returns>Data of the collection with order preserved from the 
        /// ConcurrentQueue.</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _queue.Select(t => KeyValuePair.Create(t.Key, t.Value)).GetEnumerator();
        }

        /// <summary>
        /// Removes data from the collection.
        /// </summary>
        /// <param name="key">Key of the data to remove.</param>
        /// <returns>True if the data was present and has been removed.</returns>
        public bool Remove(TKey key)
        {
            lock (_lock)
            {
                if (ContainsKey(key))
                {
                    var list = _queue.ToList();
                    list.Remove(list.FirstOrDefault(t => t.Key.Equals(key)));
                    _queue = new ConcurrentQueue<(TKey Key, TValue Value)>(list);
                    _dictionary.Remove(key);

                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Removes data from the collection.
        /// </summary>
        /// <param name="item">KeyValuePair of the data to remove.</param>
        /// <returns>True if the data was present and has been removed.</returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        /// <summary>
        /// Retrieves the data directly from the underlying
        /// ConcurrentDictionary.
        /// </summary>
        /// <param name="key">Key of the data to return.</param>
        /// <param name="value">Value of the data to return.</param>
        /// <returns>True if the Key is present and the data is returned.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Get an instance of an IEnumerator
        /// for the data in the collection.
        /// </summary>
        /// <returns>Data of the collection with order preserved from the 
        /// ConcurrentQueue.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Reorders the data in the collection according the supplied selector
        /// and ReorderDirection specified.
        /// </summary>
        /// <param name="selector">Func of the selector.</param>
        /// <param name="direction">ReorderDirection specifying the direction
        /// to sort the data.</param>
        public void Reorder(Func<(TKey Key, TValue Value), TKey> selector
            , ReorderDirection direction = ReorderDirection.Ascending)
        {
            var ordered = direction == ReorderDirection.Ascending ? _queue.OrderBy(selector) : _queue.OrderByDescending(selector);

            _queue = new ConcurrentQueue<(TKey Key, TValue Value)>(ordered);
        }

        /// <summary>
        /// Reorders the data in the collection according the supplied selector,
        /// IComparer and ReorderDirection specified.
        /// </summary>
        /// <param name="selector">Func of the selector.</param>
        /// <param name="comparer">IComparer instance that performs the test
        /// to determine the ordinality of two datum in the collection.</comparer>
        /// <param name="direction">ReorderDirection specifying the direction
        /// to sort the data.</param>
        public void Reorder(Func<(TKey Key, TValue Value), TKey> selector
            , IComparer<TKey> comparer
            , ReorderDirection direction = ReorderDirection.Ascending)
        {
            var ordered = direction == ReorderDirection.Ascending ? _queue.OrderBy(selector, comparer) : _queue.OrderByDescending(selector, comparer);

            _queue = new ConcurrentQueue<(TKey Key, TValue Value)>(ordered);
        }
    }
}


/*
 * (C) 2019 Gateway Programming School , Inc.
 */
