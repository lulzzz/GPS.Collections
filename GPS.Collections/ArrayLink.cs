using System;

namespace GPS.Collections
{
    internal class ArrayLink<T> : IDisposable
    {
        public static decimal GrowthRate = 1.25m;
        public int Lowest { get; private set; }

        public int Size { get; private set; }

        public T[] Values { get; private set; }

        public ArrayLink<T> Lower { get; set; }

        public ArrayLink<T> Higher { get; set }

        public ArrayLink(int lowest, int size)
        {
            Lowest = lowest;
            Size = size;

            Values = new T[size];
        }

        public T this[int key]
        {
            get
            {
                if (key > Lowest + Size)
                {
                    if (Higher != null) return Higher[key];

                    throw new System.IndexOutOfRangeException();
                }
                else if (key < Lowest)
                {
                    if (Lower != null) return Lower[key];

                    throw new System.IndexOutOfRangeException();
                }

                var index = Math.Abs(key) - Math.Abs(Lowest);

                return Values[index];
            }

            set
            {
                if (key < Lowest)
                {
                    if (Lower == null)
                    {
                        var growth = Size * GrowthRate;
                        int newSize = growth == (int)growth ? (int)growth : (int)growth + 1;
                        Lower = new ArrayLink<T>(Lowest - newSize, newSize);
                    }

                    Lower[key] = value;
                }
                else if (key > Lowest + Size)
                {
                    if (Higher == null)
                    {
                        var growth = Size * GrowthRate;
                        int newSize = growth == (int)growth ? (int)growth : (int)growth + 1;
                        Higher = new ArrayLink<T>(Lowest + Size, newSize);
                    }

                    Higher[key] = value;
                }

                var index = Math.Abs(key) - Math.Abs(Lowest);

                Values[index] = value;
            }
        }

        // public void Insert(int index, T value)
        // {
        //     if (index >= Lowest && index < Lowest + Size)
        //     { 
        //         if(index < Lowest + Size - 1)
        //         {
        //             for(int i = index + 1; i< Lowest + Size; ++i)
        //             {
        //                 this[i] = this[i - 1];
        //             }

        //             this[index] = value;
        //         }
        //     }
        //     else
        //     {
        //         if (index < Lowest) Lower.Insert(index, value);
        //         else Higher.Insert(index, value);
        //     }
        // }

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
