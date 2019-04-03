using System.Collections;
using System.Collections.Generic;

namespace GPS.Collections
{
    public class LinkedArrayEnumerator<T> : IEnumerator<T>
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
}
