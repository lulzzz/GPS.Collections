using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace GPS.Collections.Tests
{
    public class LinkedArray_Tests
    {
        ITestOutputHelper _log;

        public LinkedArray_Tests(ITestOutputHelper log)
        {
            _log = log;
        }

        [Fact]
        public void Constructor_Default()
        {
            var linkedArray = new LinkedArray<object>();

            Assert.NotNull(linkedArray);
        }

        [Theory]
        [ClassData(typeof(IntDataSet))]
        public void Constructor_Size((int index, int size, int count, bool higher, bool lower) data)
        {
            _log.WriteLine($"{data}");

            var list = new LinkedArray<int>(data.size);

            Assert.Equal(0, (int)list.Count);
        }

        [Theory]
        [ClassData(typeof(IntDataSet))]
        public void Constructor_SetElement((int index, int size, int count, bool higher, bool lower) data)
        {
            _log.WriteLine($"{data}");

            var list = new LinkedArray<int>(data.index, data.count);

            Assert.Equal(data.count, list[data.index]);
        }

        [Theory]
        [ClassData(typeof(IntDataSet2))]
        public void Constructor_Range((int[] set, int size, int count, bool higher, bool lower) data)
        {
            _log.WriteLine($"{data}");

            var list = new LinkedArray<int>(data.set);

            Assert.Equal(data.set.Length, list.Count);
            Assert.Equal(0, list.Lowest);
            Assert.Equal(data.set.Length - 1, list.Highest);

            for (int i = 0; i < data.set.Length; ++i)
            {
                Assert.Equal(data.set[i], list[i]);
            }
        }

        [Theory]
        [ClassData(typeof(IntDataSet))]
        public void Add((int index, int size, int count, bool higher, bool lower) data)
        {
            _log.WriteLine($"{data}");

            var list = new LinkedArray<int>();
            list.Add(data.index);

            Assert.Equal(1, (int)list.Count);
            Assert.Equal(0, list.Lowest);
            Assert.Equal(0, list.Highest);
            Assert.Equal(data.index, list[0]);
        }

        [Theory]
        [ClassData(typeof(IntDataSet2))]
        public void AddRange((int[] set, int size, int count, bool higher, bool lower) data)
        {
            _log.WriteLine($"{data}");

            var list = new LinkedArray<int>();
            list.AddRange(data.set);

            Assert.Equal(data.set.Length, (int)list.Count);
            Assert.Equal(0, list.Lowest);
            Assert.Equal(data.set.Length - 1, list.Highest);

            for (int i = 0; i < data.set.Length; ++i)
            {
                Assert.Equal(data.set[i], list[i]);
            }
        }

        [Theory]
        [ClassData(typeof(IntDataSet2))]
        public void AddRangeAt((int[] set, int size, int count, bool higher, bool lower) data)
        {
            _log.WriteLine($"{data}");

            var list = new LinkedArray<int>();
            list.AddRangeAt(-1, data.set);

            Assert.Equal(data.set.Length, (int)list.Count);
            Assert.Equal(-1, list.Lowest);
            Assert.Equal(data.set.Length - 2, list.Highest);

            for (int i = 0; i < data.set.Length; ++i)
            {
                Assert.Equal(data.set[i], list[i - 1]);
            }
        }

        [Theory]
        [ClassData(typeof(IntDataSet))]
        public void Clear((int index, int size, int count, bool higher, bool lower) data)
        {
            _log.WriteLine($"{data}");

            var list = new LinkedArray<int>(data.index, data.count);
            list.Clear();

            Assert.Equal(0, (int)list.Count);
            Assert.Equal(0, list.Lowest);
            Assert.Equal(0, list.Highest);
            Assert.Throws(typeof(IndexOutOfRangeException), () => list[data.index]);
        }

        [Fact]
        public void Contains()
        {
            var list = new LinkedArray<string>(10, "Test");
            Assert.True(list.Contains("Test"));
            Assert.False(list.Contains(""));
        }

        [Fact]
        public void IndexOf()
        {
            var list = new LinkedArray<string>(10, "Test");
            Assert.Equal(10, list.IndexOf("Test"));
            Assert.Equal(list.Lowest - 1, list.IndexOf(""));
        }
    }

    public class LinkedArrayDataSet1 : IEnumerable<object[]>
    {
        object[] _data = {
                10, 20, 30, 40, 50, 60, 70, 80, 90, 100,
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
