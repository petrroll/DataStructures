using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SplayTree
{
    class Logger
    {
        private int TreeSize;
        private long Sum;
        private long Count;

        public void Initialize(int newSize)
        {
            TreeSize = newSize;
            Sum = 0;
            Count = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddValue(int value)
        {
            checked
            {
                Sum += value;
                Count++;
            }

        }

        public void Flush()
        {
            if (Count == 0) { return; }
            Console.WriteLine($"{TreeSize} {(double)Sum / Count}");
        }
    }
}
