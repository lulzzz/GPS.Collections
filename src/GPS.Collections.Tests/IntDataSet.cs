using System.Collections;
using System.Collections.Generic;

namespace GPS.Collections.Tests
{
    public class IntDataSet : IEnumerable<object[]>
    {
        private static readonly int size = ArrayLink<object>.InitialSize;
        private static readonly int growth = (int)(size + size*ArrayLink<object>.GrowthRate);
        private readonly object[] _data = {
                  (index: 0, size: 1, count: size, higher: false, lower: false)
                , (index: 1, size: 1, count: size, higher: false, lower: false)
                , (index: size - 1, size: 1, count: size, higher: false, lower: false)
                , (index: size, size: 1, count: growth, higher: true, lower: false)
                , (index: -1, size: 1, count: growth, higher: false, lower: true)
                , (index: -(size - 1), size: 1, count: growth, higher: false, lower: true)
            };

        public IEnumerator<object[]> GetEnumerator()
        {
            foreach (var item in _data) yield return new object[] { item };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }    
}


/*
 * (C) 2019 Gateway Programming School , Inc.
 */
