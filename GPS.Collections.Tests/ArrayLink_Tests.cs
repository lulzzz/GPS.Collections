using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GPS.Collections;
using Xunit;
using Xunit.Abstractions;

namespace MyApp.Tests
{
    public class ArrayLink_Tests
    {
        ITestOutputHelper _log;

        public ArrayLink_Tests(ITestOutputHelper log)
        {
            _log = log;
        }
        
        [Theory]
        [ClassData(typeof(IntDataSet))]
        public void InitializeArrayLink((int index, int size, int count, bool higher, bool lower) data)
        {
            _log.WriteLine($"{data}");

            var link = new ArrayLink<int>(0, data.size);
            link[data.index] = data.index;
                        
            var count = (link.Lower?.Values.Length ?? 0) + 
                link.Values.Length + 
                (link.Higher?.Values.Length ?? 0);
            
            Assert.Equal(data.size, link.Highest - link.Lowest + 1);
            Assert.Equal(data.index, link[data.index]);
            Assert.Equal(data.index, link.Lowest);
            Assert.Equal(data.lower, link.Lower != null);
            Assert.Equal(data.higher, link.Higher != null);
            Assert.Equal(data.count, count);
        }
        
        [Theory]
        [ClassData(typeof(IntDataSet2))]
        public void AddSet((int[] set, int size, int count, bool higher, bool lower) data)
        {
             _log.WriteLine($"{data}");

            var link = new ArrayLink<int>(0, 1024);
            foreach(var item in data.set) link[item] = item;
                        
            var count = (link.Lower?.Values.Length ?? 0) + 
                link.Values.Length + 
                (link.Higher?.Values.Length ?? 0);

            Assert.Equal(data.size, link.Size);
            Assert.Equal(data.set.Min(i => i), link[link.Lowest]);
            Assert.Equal(data.set.Max(i => i), link[link.Highest]);
            Assert.Equal(data.set.Min(i => i), link.Lowest);
            Assert.Equal(data.set.Max(i => i), link.Highest);
            Assert.Equal(data.lower, link.Lower != null);
            Assert.Equal(data.higher, link.Higher != null);
            Assert.Equal(data.count, count);
        }
    }

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
