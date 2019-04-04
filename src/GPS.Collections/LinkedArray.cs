/*
    # GPS.Collections
    
    ## LinkedArray.cs

    Data structure that comprises an implementation of 
    ICollection<T> that is backed by the ArrayLink object.

    ## Copyright

    2019 - Gateway Programming School, Inc.
    
    This notice must be retained for any use the code
    herein in whole or in part for any use.
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace GPS.Collections
{
    /// <summary>
    /// Collection of generic values T that 
    /// uses a linked list of ArrayLink&lt;T&gt;
    /// instances holding the values of the collection.
    /// </summary>
    /// <typeparam name="T">Type of data held in the collection.</typeparam>
    public sealed class LinkedArray<T> : ICollection<T>, IList<T>, IDisposable
    {
        /// <summary>
        /// Root ArrayLink&lt;T&gt;
        /// </summary>
        private ArrayLink<T> _root = null;

        /// <summary>
        /// Capacity of the Root
        /// </summary>
        private int _size => _root.Size;

        /// <summary>
        /// Lowest value in the Collection
        /// </summary>
        public int Lowest => _root.Lowest;

        /// <summary>
        /// Highest value in the Collection
        /// </summary>
        public int Highest => _root.Highest;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks>
        /// Initializes the collection with a
        /// root ArrayLink&lt;T&gt;.
        /// </remarks>
        public LinkedArray()
        {
            _root = new ArrayLink<T>(0, 1024);
        }

        /// <summary>
        /// Constructor that takes a key and value
        /// of a piece of data being initially added
        /// to the collection.
        /// </summary>
        /// <param name="index">Key of the value being added.</param>
        /// <param name="value">Value to be added.</param>
        /// <returns></returns>
        public LinkedArray(int index, T value) : this()
        {
            _root[index] = value;
        }

        /// <summary>
        /// Constructor that creates an ArrayLink&lt;T&gt; of
        /// the requested size.
        /// </summary>
        /// <param name="initialSize">Requested size of the Collection.</param>
        public LinkedArray(int initialSize)
        {
            _root = new ArrayLink<T>(0, initialSize);
        }

        /// <summary>
        /// Constructor that accepts an ICollection&lt;T&gt; containing
        /// values to initialize the collection starting at key = 0.
        /// </summary>
        /// <param name="collection">Values to initialize the collection.</param>
        public LinkedArray(ICollection<T> collection)
        {
            _root = new ArrayLink<T>(0, Math.Max(collection.Count, 1024));

            AddRange(collection);
        }

        /// <summary>
        /// Adds a range represented by an instance of
        /// ICollection&lt;T&gt; to add to the collection
        /// at the current Highest + 1 index.
        /// </summary>
        /// <param name="collection">Values to add to the collection.</param>
        public void AddRange(ICollection<T> collection)
        {
            AddRangeAt(_root.Initialized ? Highest + 1 : 0, collection);
        }

        /// <summary>
        /// Adds a range represented by an instance of
        /// ICollection&lt;T&gt; to add to the collection
        /// at the specific relative index requested.
        /// </summary>
        /// <param name="index">Logical index to start adding the collection</param>
        /// <param name="collection">Values to add to the collection.</param>
        /// <remarks>
        /// Existing values will be overwritten where the intersect with
        /// the indices of the inbound collection.
        /// </remarks>
        public void AddRangeAt(int index, ICollection<T> collection)
        {
            if (collection is IList<T> list)
            {
                for (int i = index; i < index + collection.Count; ++i)
                {
                    this[i] = list[i - index];
                }
            }
            else
            {
                foreach (var item in collection)
                {
                    this[index++] = item;
                }
            }
        }

        /// <summary>
        /// Gets or Sets values in the collection.
        /// </summary>
        /// <value>Value of T</value>
        public T this[int index]
        {
            get => _root[index];
            set
            {
                _root[index] = value;
            }
        }

        /// <summary>
        /// Size of the collection.
        /// </summary>
        public int Count => _size;

        /// <summary>
        /// Flag indicated the writability of the collection.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Adds a single value T at the end of the collection.
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            AddRange(new[] { item });
        }

        /// <summary>
        /// Resets the collection to its default state.
        /// </summary>
        public void Clear()
        {
            _root.Dispose();
            _root = new ArrayLink<T>(0, 1024);
        }

        /// <summary>
        /// Indicates that the collection contains the specified value.
        /// </summary>
        /// <param name="item">Value of T to search for.</param>
        /// <returns>True if the value is found, otherwise false.</returns>
        public bool Contains(T item)
        {
            return IndexOf(item) >= Lowest;
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException(
                "Cannot preserve spatial integrity");
        }

        /// <summary>
        /// Returns the <see cref="LinkedArrayEnumerator{T}"/> for the collection.
        /// </summary>
        /// <returns>IEnumerator&lt;T&gt;</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new LinkedArrayEnumerator<T>(this);
        }

        /// <summary>
        /// Returns the logical index of the specified value.
        /// </summary>
        /// <param name="item">Value to search for.</param>
        /// <returns>Logical index of the value.  Returns Lowest - 1 if the value
        /// is not found.</returns>
        public int IndexOf(T item)
        {
            for (int i = Lowest; i <= Highest; ++i)
            {
                if (_root[i].Equals(item)) return i;
            }

            return _root.Lowest - 1;
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, T item)
        {
            throw new InvalidOperationException(
                "LinkedArray<T> cannot change indices of elements.");
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            throw new InvalidOperationException(
                "LinkedArray<T> cannot change indices of elements.");
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            throw new InvalidOperationException(
                "LinkedArray<T> cannot change indices of elements.");
        }

        /// <summary>
        /// Explicit interface reference to get the Enumerator.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _root.Dispose();
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Disposable pattern.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
