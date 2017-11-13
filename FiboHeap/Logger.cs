using System;
using System.Runtime.CompilerServices;

namespace FiboHeap
{
    public class Logger
    {
        int treeSize;
        ulong sum;
        ulong count;

        public void Initialize(int newSize)
        {
            treeSize = newSize;
            sum = 0;
            count = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddValue(uint value)
        {
            checked
            {
                sum += value;
            }

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddNumberOfOperations()
        {
            checked
            {
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
