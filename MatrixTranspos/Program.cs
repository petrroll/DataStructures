#define NAIVE

using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace MatrixTranspos
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 54; i < 120; i++)
            {
                int n = (int)Math.Pow(2, (i / 9d));
                TestMatrixN(n);
            }
            Console.ReadLine();
        }

        static void TestMatrixN(int n)
        {
            int runForReal = Math.Min(4, 1024 - (n));
            int warmpUp = Math.Min(1, runForReal / 10);

            int[] m = new int[n * n];
            Stopwatch stpw = new Stopwatch();

            GC.Collect();
            runMatrixTransposition(m, n, warmpUp);

            stpw.Start();
            runMatrixTransposition(m, n, runForReal);
            stpw.Stop();

            double timePerMemoryAccess = stpw.ElapsedMilliseconds / (double)runForReal;
            int numberOfSwaps = (n * n - n) / 2;
            double timePerSwap = timePerMemoryAccess / numberOfSwaps;

            Console.WriteLine($"{n} {timePerSwap.ToString(CultureInfo.GetCultureInfo("en-US"))}");

        }

        private static void runMatrixTransposition(int[] m, int n, int numberOfRuns)
        {
            for (int i = 0; i < numberOfRuns; i++)
            {
#if NAIVE
                Transpositor3000.TransposeNaive(m, n);

#else
                Transpositor3000.Transpose(m, n);
#endif
            }
        }
    }
}
