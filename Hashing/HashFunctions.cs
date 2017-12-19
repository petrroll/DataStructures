using System;
using System.Diagnostics;

namespace Hashing
{
    public interface IHashFunction
    {
        int Hash(ulong input);
        void Reset();
    }

    public sealed class MultiShiftHash : IHashFunction
    {
        Random rnd = new Random();

        private readonly int mSizeBits;
        private static readonly int uSizeBits = 64;

        private int numberOfForgottenBits;
        private ulong a;

        public MultiShiftHash(int mSizeBits)
        {
            this.mSizeBits = mSizeBits;
            this.numberOfForgottenBits = (uSizeBits - mSizeBits); Reset();
        }


        public int Hash(ulong input)
        {
            // No need to do modulo |U| as our universe it of the same size as our data type (64bit)         
            ulong result = (a * input) >> numberOfForgottenBits;

            Debug.Assert(result < (1ul << mSizeBits)); // Assume mSizeBits < 64, otherwise this assert will fire everytime
            return (int)result;
        }

        // Select a random multiplication constant from the universum.
        public void Reset()
        {
            a = rnd.NextULong();
        }
    }

    public sealed class NaiveModuloHash : IHashFunction
    {
        private readonly int mSizeBits;
        private ulong mask = 0;

        public NaiveModuloHash(int mSizeBits)
        {
            this.mSizeBits = mSizeBits;

            // mSizeBits must be < 64 otherwise the mask will be wrong
            this.mask = (ulong)((1 << mSizeBits) - 1);
        }

        public int Hash(ulong input)
        {
            // since mSize is 2^k the modulo op is taking mSizeBits lower bits of the input
            ulong result = input & mask;

            Debug.Assert(result < (1ul << mSizeBits)); // Assume mSizeBits < 64, otherwise this assert will fire everytime
            return (int)result;
        }

        public void Reset(){}
    }

    public sealed class TableHash : IHashFunction
    {
        Random rnd = new Random();

        private readonly int mSizeBits;
        private readonly int tableIndexSizeBits;
        private readonly ulong[][] tables;

        ulong mask;

        public TableHash(int mSizeBits, int tableSizeBits)
        {
            this.tableIndexSizeBits = tableSizeBits;
            this.mSizeBits = mSizeBits;

            // Number of tables is celing(UniversumBits / TableSize)
            int numberOfTables = (64 / tableSizeBits) + (64 % tableSizeBits == 0 ? 0 : 1);

            // allocate all tables in the beginning
            tables = new ulong[numberOfTables][];
            for (int i = 0; i < numberOfTables; i++)
            {
                tables[i] = new ulong[1 << tableSizeBits];
            }

            mask = (ulong)(1 << tableIndexSizeBits) - 1;
            Reset();
        }

        public int Hash(ulong input)
        {
            Debug.Assert(mask <= int.MaxValue);

            ulong result = 0;
            for (int i = 0; i < tables.Length; i++)
            { 
                int index = (int)(input & mask);

                Debug.Assert((ulong)index < (1ul << tableIndexSizeBits));
                result = result ^ tables[i][index];

                // shift the bits we used for the last table away
                input = input >> tableIndexSizeBits;
            }

            Debug.Assert(result < (1ul << mSizeBits)); // Assume mSizeBits < 64, otherwise this assert will fire everytime
            return (int)result;
        }

        public void Reset()
        {
            // Assign the tables with random values.
            for (int i = 0; i < tables.Length; i++)
            {
                for (int j = 0; j < tables[i].Length; j++)
                {
                    tables[i][j] = (ulong)rnd.Next(1 << mSizeBits);
                }
            }
        }
    }

    public static class Helpers
    {
        static byte[] buffer = new byte[8];
        public static ulong NextULong(this Random rnd)
        {
            rnd.NextBytes(buffer);
            return BitConverter.ToUInt64(buffer, 0);
        }
    }
}
