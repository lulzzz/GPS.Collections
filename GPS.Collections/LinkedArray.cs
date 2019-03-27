using System;
using System.Collections;
using System.Collections.Generic;

namespace GPS.Collections
{
    public sealed class LinkedArray<T> : ICollection<T>, IList<T>, IDisposable
    {
        private ArrayLink<T> _root = null;

        private int _size = 0;

        public int Lowest { get; private set; } = 0;

        public int Highest => _size - Lowest;

        public LinkedArray()
        {
            _root = new ArrayLink<T>(0, 1000);
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
            _root = new ArrayLink<T>(0, Math.Max(collection.Count, 1000));

            AddRange(collection);
        }

        public void AddRange(ICollection<T> collection)
        {
            AddRangeAt(_size, collection);
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

            _size += collection.Count;
        }
        public T this[int index]
        {
            get => _root[index];
            set
            {
                _root[index] = value;

                if (index < Lowest) Lowest = index;
                if (index > _size) _size = index + 1;
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
            _root = new ArrayLink<T>(0, 1000);
        }

        public bool Contains(T item)
        {
            return IndexOf(item) > int.MinValue;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array.Length - arrayIndex < _size)
                throw new ArgumentOutOfRangeException(nameof(array));

            for (int i = Lowest; i < _size; ++i)
            {
                array[arrayIndex + Math.Abs(i) - Math.Abs(Lowest)] = _root[i];
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new LinkedArrayEnumerator(this);
        }

        public class LinkedArrayEnumerator : IEnumerator<T>
        {
            private int _index = int.MinValue;
            private LinkedArray<T> _values = null;

            public LinkedArrayEnumerator(LinkedArray<T> values)
            {
                _values = values;
            }

            public T Current =>
                _index != int.MinValue
                    ? _values[_index]
                    : default(T);

            object IEnumerator.Current => this.Current;

            public void Dispose()
            {
                _values = null;
            }

            public bool MoveNext()
            {
                ++_index;

                if (_index == _values.Highest + 1) return false;

                return true;
            }

            public void Reset()
            {
                _index = int.MinValue;
            }
        }

        public int IndexOf(T item)
        {
            for (int i = Lowest; i < _size; ++i)
            {
                if (_root[i].Equals(item)) return i;
            }

            return int.MinValue;
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
