using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Xunit;
using Xunit.Abstractions;

namespace GPS.Collections.Tests
{
    public class OrderedConcurrentDictionary_Tests
    {
        readonly ITestOutputHelper _log;

        public OrderedConcurrentDictionary_Tests(ITestOutputHelper log)
        {
            _log = log;
        }

        [Theory]
        [ClassData(typeof(OrderedConcurrentDictionaryComparisonSet))]
        public void IsOrderPreserved(Dictionary<int, string> values)
        {
            var ocd = new OrderedConcurrentDictionary<int, string>(values);

            var original = values.ToArray();
            var data = ocd.ToArray();

            Assert.Equal(original.Length, data.Length);

            for(int i =0; i < original.Length; ++i)
            {
                _log.WriteLine($"[{original[i].Key}, {original[i].Value}] : [{data[i].Key}, {data[i].Value}]");
                Assert.Equal(original[i].Key, data[i].Key);
                Assert.Equal(original[i].Value, data[i].Value);
            }
        }

        [Theory]
        [ClassData(typeof(OrderedConcurrentDictionaryComparisonSet))]
        public void IsSorted(Dictionary<int, string> values)
        {
            var ocd = new OrderedConcurrentDictionary<int, string>(values);
            ocd.Reorder(t => t.Key);

            var original = values.OrderBy(p => p.Key).ToArray();
            var data = ocd.ToArray();

            Assert.Equal(original.Length, data.Length);

            for(int i =0; i < original.Length; ++i)
            {
                _log.WriteLine($"[{original[i].Key}, {original[i].Value}] : [{data[i].Key}, {data[i].Value}]");
                Assert.Equal(original[i].Key, data[i].Key);
                Assert.Equal(original[i].Value, data[i].Value);
            }
        }

        [Theory]
        [ClassData(typeof(OrderedConcurrentDictionaryComparisonSet))]
        public void IsSortedDescending(Dictionary<int, string> values)
        {
            var ocd = new OrderedConcurrentDictionary<int, string>(values);
            ocd.Reorder(t => t.Key, ReorderDirection.Descending);

            var original = values.OrderByDescending(p => p.Key).ToArray();
            var data = ocd.ToArray();

            Assert.Equal(original.Length, data.Length);

            for(int i =0; i < original.Length; ++i)
            {
                _log.WriteLine($"[{original[i].Key}, {original[i].Value}] : [{data[i].Key}, {data[i].Value}]");
                Assert.Equal(original[i].Key, data[i].Key);
                Assert.Equal(original[i].Value, data[i].Value);
            }
        }
    }
}


/*
 * (C) 2019 Gateway Programming School , Inc.
 */
