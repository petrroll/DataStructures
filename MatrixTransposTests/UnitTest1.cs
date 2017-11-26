using System;
using Xunit;
using MatrixTranspos;

namespace MatrixTransposTests
{
    public class MatrixTests
    {
        public static void TransposeNaive(int[] matrix, int n)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = i; j < n; j++)
                {
                    swap(matrix, i * n + j, j * n + i);
                }
            }

            void swap(int[] m, int i, int j)
            {
                int tmp = m[i];

                m[i] = m[j];
                m[j] = tmp;
            }
        }

        [Fact]
        public void Test1()
        {
            int[] matrix = { 1 };
            int n = (int)Math.Sqrt(matrix.Length);
            var naiveMatrix = (int[])matrix.Clone();

            Transpositor3000.Transpose(matrix, n);
            TransposeNaive(naiveMatrix, n);

            Assert.Equal(naiveMatrix, naiveMatrix);
        }

        [Fact]
        public void Test2()
        {
            int[] matrix = { 1, 2, 3, 4};
            int n = (int)Math.Sqrt(matrix.Length);
            var naiveMatrix = (int[])matrix.Clone();

            Transpositor3000.Transpose(matrix, n);
            TransposeNaive(naiveMatrix, n);

            Assert.Equal(naiveMatrix, naiveMatrix);
        }

        [Fact]
        public void Test3()
        {
            int[] matrix = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int n = (int)Math.Sqrt(matrix.Length);

            var naiveMatrix = (int[])matrix.Clone();

            Transpositor3000.Transpose(matrix, n);
            TransposeNaive(naiveMatrix, n);

            Assert.Equal(naiveMatrix, naiveMatrix);
        }

        [Theory]
        [InlineData(17)]
        [InlineData(34)]
        [InlineData(64)]
        public void Test4(int n)
        {
            int[] matrix = new int[n * n];

            for (int i = 0; i < n * n; i++)
            {
                matrix[i] = i;
            }

            var naiveMatrix = (int[])matrix.Clone();

            Transpositor3000.Transpose(matrix, n);
            TransposeNaive(naiveMatrix, n);

            Assert.Equal(naiveMatrix, naiveMatrix);
        }
    }
}
