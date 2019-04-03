using System.Collections;
using System.Collections.Generic;

namespace GPS.Collections.Tests
{
    public class IntDataSet : IEnumerable<object[]>
    {
        private const int growth = (int)(1024 + 1024*1.25);
        private object[] _data = {
                  (index: 0, size: 1, count: 1024, higher: false, lower: false)
                , (index: 1, size: 1, count: 1024, higher: false, lower: false)
                , (index: 1023, size: 1, count: 1024, higher: false, lower: false)
                , (index: 1024, size: 1, count: growth, higher: true, lower: false)
                , (index: -1, size: 1, count: growth, higher: false, lower: true)
                , (index: -1023, size: 1, count: growth, higher: false, lower: true)
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
