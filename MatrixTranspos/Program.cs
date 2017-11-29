#define NAIVE
//#define SIMULATOR

using System;
using System.Diagnostics;
using System.Globalization;

namespace MatrixTranspos
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 54; i < 140; i++)
            {
                int n = (int)Math.Ceiling(Math.Pow(2, (i / 9d)));
                TestMatrixN(n);
            }
        }

        static void TestMatrixN(int n)
        {
            int[] m = null;
#if SIMULATOR
            int runForReal = 1;
            int warmpUp = 0;
#else
            int runForReal = Math.Max(4, 1024 - (n));
            if (n >= 20643) { runForReal = 2; }

            int warmpUp = Math.Max(1, runForReal / 10);


            m = new int[n * n];

            Stopwatch stpw = new Stopwatch();
#endif
            GC.Collect();
            runMatrixTransposition(m, n, warmpUp);

#if SIMULATOR
            Console.WriteLine($"N {n}");
#else
            stpw.Start();
#endif
            runMatrixTransposition(m, n, runForReal);

#if SIMULATOR
            Console.WriteLine($"E");
            Console.WriteLine();

#else
            stpw.Stop();
            double timePerMemoryAccess = stpw.ElapsedMilliseconds / (double)runForReal;
            int numberOfSwaps = (n * n - n) / 2;
            double timePerSwap = timePerMemoryAccess / numberOfSwaps;

            Console.WriteLine($"{n} {timePerSwap.ToString(CultureInfo.GetCultureInfo("en-US"))}");
#endif
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
