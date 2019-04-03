/*
    # GPS.Collections
    
    ## LinkedArrayEnumerator.cs

    Iterates over the LinkedArray data structure.

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
    public class LinkedArrayEnumerator<T> : IEnumerator<T>, IDisposable
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
