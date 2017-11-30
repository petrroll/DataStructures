//#define SIMULATOR
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MatrixTranspos
{
    public static class Transpositor3000
    {
        public const int RecursionEnd = 8;
        public static void TransposeNaive(int[] matrix, int n)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = i; j < n; j++)
                {
                    swap(matrix, i * n + j, j * n + i);
                }
            }
        }

        public static void TransposeNaive(int[] matrix, int leftUpIndex, int size, int n)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = i; j < size; j++)
                {
                    swap(matrix, leftUpIndex + i * n + j, leftUpIndex + j * n + i);
                }
            }
        }



        public static void Transpose(int[] matrix, int n)
        {
            Debug.Assert(matrix.Length == n * n);
            Transpose(matrix, 0, n, n);
        }

        private static void Transpose(int[] matrix, int leftUpIndex, int size, int n)
        {
            Debug.Assert(size > 0);

            // No need to transpose 1x1 matrix
            if (size <= RecursionEnd) { TransposeNaive(matrix, leftUpIndex, size, n); }
            else
            {

                int smallerSquareN = size / 2;
                int biggerSquareN = size - smallerSquareN;

                int rightDownIndex = leftUpIndex + biggerSquareN * n + biggerSquareN;

                Transpose(matrix, leftUpIndex, biggerSquareN, n);
                Transpose(matrix, rightDownIndex, smallerSquareN, n);

                int leftDownIndex = leftUpIndex + biggerSquareN * n;

                TransposeAndSwitch(matrix, leftDownIndex, (biggerSquareN, smallerSquareN), n);
            }

        }

        private static void TransposeAndSwitch(int[] matrix, int leftUpIndex, (int w, int h) size, int n)
        {

            // If matrix <= 8x8 or 9x8 proceed with naive transpose & switch
            if (size.h <= RecursionEnd)
            {
                TransposeAndSwitchNaive(matrix, n, leftUpIndex, size);
            }
            else
            {
                int hSmaller = (size.h) / 2;
                int hBigger = size.h - hSmaller;

                int wSmaller = (size.w) / 2;
                int wBigger = size.w - wSmaller;

                TransposeAndSwitch(matrix, leftUpIndex, (wBigger, hBigger), n);
                TransposeAndSwitch(matrix, leftUpIndex + wBigger, (wSmaller, hBigger), n);
                TransposeAndSwitch(matrix, leftUpIndex + hBigger * n, (wBigger, hSmaller), n);
                TransposeAndSwitch(matrix, leftUpIndex + hBigger * n + wBigger, (wSmaller, hSmaller), n);

            }

        }

        static void TransposeAndSwitchNaive(int[] matrix, int n, int startIndex, (int w, int h) size)
        {
            int secondTileStartIndex = (startIndex % n) * n + (startIndex / n);

            for (int i = 0; i < size.h; i++)
            {
                int l = startIndex + i * n;
                int m = secondTileStartIndex + i;

                for (int j = 0; j < size.w; j++, l++, m += n)
                {
#if SIMULATOR
                    swapSimulator(matrix, n, l, m);

#else
                    swap(matrix, l, m);

#endif
                }
            }
        }

        private static void swap(int[] m, int i, int j)
        {
            Debug.Assert(
                i % (int)Math.Sqrt(m.Length) == j / (int)Math.Sqrt(m.Length) &&
                j % (int)Math.Sqrt(m.Length) == i / (int)Math.Sqrt(m.Length)
                );
            int tmp = m[i];

            m[i] = m[j];
            m[j] = tmp;
        }

        private static void swapSimulator(int[] m, int n, int i, int j)
        {
            Debug.Assert(
                i % (int)Math.Sqrt(m.Length) == j / (int)Math.Sqrt(m.Length) &&
                j % (int)Math.Sqrt(m.Length) == i / (int)Math.Sqrt(m.Length)
            );

            int x = i % n;
            int y = i / n;

            int l = j % n;
            int k = j / n;

            Console.WriteLine($"X {x} {y} {l} {k}");
        }

        public static string ToString(int[] matrix, int n)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < matrix.Length; i++)
            {
                sb.Append(matrix[i]);
                if (i % n == n - 1)
                {
                    sb.Append("\n");
                }
                else
                {
                    sb.Append(" ");
                }
            }

            return sb.ToString();
        }

    }

}
