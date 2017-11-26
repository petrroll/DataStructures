using System;
using System.Diagnostics;
using System.Text;

namespace MatrixTranspos
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            int[] matrix = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            Transpositor3000.Transpose(matrix, 3);
            Console.WriteLine(Transpositor3000.ToString(matrix, 3));
        }
    }

    static class Transpositor3000
    {
        public static string ToString(int[] matrix, int n)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < matrix.Length; i++)
            {
                sb.Append(matrix[i]);
                if (i % n == n-1)
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

        public static void Transpose(int[] matrix, int n)
        {
            Debug.Assert(matrix.Length == n * n);
            Transpose(matrix, 0, n, n);
        }

        // change leftUp to index into the matrix array ?
        private static void Transpose(int[] matrix, int leftUpIndex, int size, int n)
        {
            Debug.Assert(size > 0);

            // No need to transpose 1x1 matrix
            if (size == 1) { return; }

            int smallerSquareN = size / 2;
            int biggerSquareN = size - smallerSquareN;

            int rightDownIndex = leftUpIndex + biggerSquareN * n + biggerSquareN;

            Transpose(matrix, leftUpIndex, biggerSquareN, n);
            Transpose(matrix, rightDownIndex, smallerSquareN, n);

            int rightUpIndex = leftUpIndex + biggerSquareN;
            int leftDownIndex = leftUpIndex + biggerSquareN * n;

            TransposeAndSwitch(matrix, leftDownIndex, rightUpIndex, (biggerSquareN, smallerSquareN), n);
        }

        private static void TransposeAndSwitch(int[] matrix, int leftUpLonger, int leftUpHigher, (int w, int h) sizeLonger, int n)
        {
            Debug.Assert(sizeLonger.w >= sizeLonger.h);
            Debug.Assert(sizeLonger.w - sizeLonger.h <= 1);
            Debug.Assert(sizeLonger.h > 0);

            // Two matrixes 1x1 or 1x2 (and 2x1 respectively) -> swap elements
            if(sizeLonger.h == 1)
            {
                swap(matrix, leftUpLonger, leftUpHigher);
                if (sizeLonger.w == 2) { swap(matrix, leftUpLonger + 1, leftUpHigher + n); }
            }
            else
            {
                int squareSize = (sizeLonger.h + 1) / 2;

                int rectH = sizeLonger.h - squareSize;
                int rectW = sizeLonger.w - squareSize;


                TransposeAndSwitch(matrix, leftUpLonger, leftUpHigher, (squareSize, squareSize), n);

                TransposeAndSwitch(matrix, leftUpLonger + squareSize, leftUpHigher + (squareSize * n), (rectW, rectH), n);
                TransposeAndSwitch(matrix, leftUpLonger + squareSize * n, leftUpHigher + squareSize, (rectW, rectH), n);

                TransposeAndSwitch(matrix, leftUpLonger + squareSize * n + rectW, leftUpHigher + rectW * n + squareSize, (squareSize, squareSize), n);
            }

            // Recurse to 4 TransposeAndSwitch

            void swap(int[] m, int i, int j)
            {
                int tmp = m[i];

                m[i] = m[j];
                m[j] = tmp;
            }
        }
    }


}
