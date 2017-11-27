using System;
using System.Diagnostics;
using System.Text;

namespace MatrixTranspos
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    public static class Transpositor3000
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
                int squareSizeLeftUp = (sizeLonger.h + 1) / 2;
                int squareSizeRightDown = (sizeLonger.w >= squareSizeLeftUp * 2) ? squareSizeLeftUp : (sizeLonger.h - squareSizeLeftUp);

                int rectLeftDownH = sizeLonger.h - squareSizeLeftUp;
                int rectLeftDownW = sizeLonger.w - squareSizeRightDown;

                int rectRightUpH = sizeLonger.h - squareSizeRightDown;
                int rectRightUpW = sizeLonger.w - squareSizeLeftUp;

                TransposeAndSwitch(matrix, leftUpLonger, leftUpHigher, (squareSizeLeftUp, squareSizeLeftUp), n);

                if (rectRightUpH > rectRightUpW)
                { TransposeAndSwitch(matrix, leftUpHigher + (squareSizeLeftUp * n), leftUpLonger + squareSizeLeftUp, (rectLeftDownW, rectLeftDownH), n); }
                else
                { TransposeAndSwitch(matrix, leftUpLonger + squareSizeLeftUp, leftUpHigher + (squareSizeLeftUp * n), (rectLeftDownW, rectLeftDownH), n); }

                TransposeAndSwitch(matrix, leftUpLonger + squareSizeLeftUp * n, leftUpHigher + squareSizeLeftUp, (rectLeftDownW, rectLeftDownH), n);

                TransposeAndSwitch(matrix, leftUpLonger + rectRightUpH * n + rectLeftDownW, leftUpHigher + rectLeftDownW * n + rectRightUpH, (squareSizeRightDown, squareSizeRightDown), n);
            }

            // Recurse to 4 TransposeAndSwitch

            void swap(int[] m, int i, int j)
            {
                Debug.Assert(
                    i % (int)Math.Sqrt(m.Length) == j / (int)Math.Sqrt(m.Length) &&
                    j % (int)Math.Sqrt(m.Length) == i / (int)Math.Sqrt(m.Length)
                    );
                int tmp = m[i];

                m[i] = m[j];
                m[j] = tmp;
            }
        }
    }


}
