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
    public sealed partial class LinkedArray<T> : ICollection<T>, IList<T>, IDisposable
    {
        private ArrayLink<T> _root = null;

        private int _size => _root.Size;

        public int Lowest => _root.Lowest;

        public int Highest => _root.Highest;

        public LinkedArray()
        {
            _root = new ArrayLink<T>(0, 1024);
        }

        public LinkedArray(int index, T value) : this()
        {
            _root[index] = value;
        }

        public LinkedArray(int initialSize)
        {
            _root = new ArrayLink<T>(0, initialSize);
        }

        public LinkedArray(ICollection<T> collection)
        {
            _root = new ArrayLink<T>(0, Math.Max(collection.Count, 1024));

            AddRange(collection);
        }

        public void AddRange(ICollection<T> collection)
        {
            AddRangeAt(Highest, collection);
        }

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
        public T this[int index]
        {
            get => _root[index];
            set
            {
                _root[index] = value;
            }
        }

        public int Count => _size;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            AddRange(new[] { item });
        }

        public void Clear()
        {
            _root.Dispose();
            _root = new ArrayLink<T>(0, 1024);
        }

        public bool Contains(T item)
        {
            return IndexOf(item) >= Lowest;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException(
                "Cannot preserve spatial integrity");
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new LinkedArrayEnumerator<T>(this);
        }

        public int IndexOf(T item)
        {
            for (int i = Lowest; i <= Highest; ++i)
            {
                if (_root[i].Equals(item)) return i;
            }

            return _root.Lowest - 1;
        }

        public void Insert(int index, T item)
        {
            throw new InvalidOperationException(
                "LinkedArray<T> cannot change indices of elements.");
        }

        public bool Remove(T item)
        {
            throw new InvalidOperationException(
                "LinkedArray<T> cannot change indices of elements.");
        }

        public void RemoveAt(int index)
        {
            throw new InvalidOperationException(
                "LinkedArray<T> cannot change indices of elements.");
        }

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

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~LinkedArray() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
