using System;

namespace GPS.Collections
{
    public class ArrayLink<T> : IDisposable
    {
        public static decimal GrowthRate = 1.25m;

        public int Start { get; private set; }

        public int Lowest { get; private set; }

        public int Highest { get; private set; }

        private bool _initialized = false;

        public int Size {
            get{
                var a = Math.Abs(Lowest);
                var b = Math.Abs(Highest);
                return (Lowest < 0 
                    ? (Highest > 0 ? b + a : a - b)
                    : b - a) + 1;
            }
        }

        public T[] Values { get; private set; }

        public ArrayLink<T> Lower { get; set; }

        public ArrayLink<T> Higher { get; set; }

        public ArrayLink(int start, int size)
        {
            Start = start;
            size = Math.Abs(size);
            if (size < 1024) size = 1024;
            Values = new T[size];
        }

        public T this[int key]
        {
            get
            {
                if (key >= Start + Values.Length)
                {
                    if (Higher != null) return Higher[key];

                    throw new System.IndexOutOfRangeException();
                }
                else if (key < Start)
                {
                    if (Lower != null) return Lower[key];

                    throw new System.IndexOutOfRangeException();
                }

                var a = Math.Abs(key);
                var b = Math.Abs(Start);
                var index = key < 0 ? b - a : a - b; 

                return Values[index];
            }

            set
            {
                Func<int> newSize = () =>
                {
                    var temp = Values.Length * GrowthRate;
                    return temp == (int)temp ? (int)temp : (int)temp + 1;
                };

                if (!_initialized)
                {
                    Lowest = key;
                    Highest = key;
                    _initialized = true;
                }
                
                if (key < Start)
                {
                    if (Lower == null)
                    {
                        var size = newSize();
                        Lower = new ArrayLink<T>(Start - size, size);
                    }

                    Lowest = Math.Min(key, Lowest);

                    Lower[key] = value;
                    return;
                }
                else if (key >= Start + Values.Length)
                {
                    if (Higher == null)
                    {
                        var size = newSize();
                        Higher = new ArrayLink<T>(Start + Values.Length, size);
                    }

                    Highest = Math.Max(key, Highest);
                    Higher[key] = value;
                    return;
                }

                var a = Math.Abs(key);
                var b = Math.Abs(Start);
                var index = key < 0 ? b - a : a - b;  

                Lowest = Math.Min(key, Lowest);
                Highest = Math.Max(key, Highest);                 

                Values[index] = value;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Values = null;
                    if (Lowest <= 0) Lower?.Dispose();
                    else Higher?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ArrayLink() {
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
