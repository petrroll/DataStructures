using System;
using System.Runtime.CompilerServices;

namespace SplayTree
{
    class Logger
    {
        int treeSize;
        long sum;
        long count;

        public void Initialize(int newSize)
        {
            treeSize = newSize;
            sum = 0;
            count = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddValue(int value)
        {
            checked
            {
                sum += value;
                count++;
            }

        }

        public void Flush()
        {
            if (count == 0) { return; }
            double averageLength = (double)sum / count;
            Console.WriteLine($"{treeSize} {averageLength}");
        }
    }
}
