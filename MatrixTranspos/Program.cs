//#define NAIVE
#define SIMULATOR

using System;
using System.Diagnostics;
using System.Globalization;

namespace MatrixTranspos
{
    class Program
    {
        static void Main(string[] args)
        {
#if SIMULATOR
            for (int i = 54; i < 130; i++) // Simulator does not like larger matrixes
#else
            for (int i = 54; i < 140; i++) // For 140 the result of n*n is > Int32.Max()

#endif
            {
                int n = (int)Math.Ceiling(Math.Pow(2, (i / 9d)));
                Console.Error.WriteLine($"Currently working on: {i | n}");
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
            // Run for a reasonable number of iterations so that the results are relatively stable. As the
            // .. matrix size increases decrease the number of runs to 4 / 2 at minimum for very large matrixes.

            // An adaptive approach that computes variance and runs enough times so that the variance is small 
            // .. was considered but deemed unnecessary as this static approach works just fine.
            int runForReal = Math.Max(4, 1024 - (n));
            if (n >= 20643) { runForReal = 2; }

            // If not in simualator let's do a few warmpup runs first for JITing, prefetch and branch prediction warmup, ...
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
