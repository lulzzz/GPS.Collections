using System.Collections;
using System.Collections.Generic;

namespace GPS.Collections.Tests
{
    public class IntDataSet2 : IEnumerable<object[]>
    {
        private const int growth = (int)(1024 + 1024*1.25m);
        private object[] _data = {
                  (set: new [] { -1, -2, -3 }, size: 3, count: growth, higher: false, lower: true)
                , (set: new [] { 0, 1, 2 }, size: 3, count: 1024, higher: false, lower: false)
                , (set: new [] { 1023, 1024 }, size: 2, count: growth, higher: true, lower: false)
                , (set: new [] { -1023, 1024 }, size: 2048, count: growth + (int)(1024*1.25m), higher: true, lower: true)
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
 * (C) 2019 Your Legal Entity's Name
 */
