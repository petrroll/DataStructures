using System;
using System.Diagnostics;

namespace Hashing
{

    class MultiShiftHash
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


        public ulong Hash(ulong input)
        {
            // No need to do modulo |U| as our universe it of the same size as our data type          
            ulong result = (a * input) >> numberOfForgottenBits;
            Debug.Assert(result < (1ul << mSizeBits)); // Assume mSizeBits < 64, otherwise this assert will fire everytime

            return result;
        }

        public void Reset()
        {
            a = rnd.NextULong();
        }
    }

    class NaiveModuloHash
    {
        private readonly int mSizeBits;
        private ulong mask = 0;

        public NaiveModuloHash(int mSizeBits)
        {
            this.mSizeBits = mSizeBits;

            // mSizeBits must be < 64 otherwise the mask will be wrong
            this.mask = (ulong)((1 << mSizeBits) - 1);
        }

        public ulong Hash(ulong input)
        {
            ulong result = input & mask;
            Debug.Assert(result < (1ul << mSizeBits)); // Assume mSizeBits < 64, otherwise this assert will fire everytime

            return result;
        }
    }

    class TableHash
    {
        Random rnd = new Random();

        private readonly int mSizeBits;
        private readonly int tableIndexSizeBits;
        private readonly ulong[][] tables;

        public TableHash(int mSizeBits, int tableSizeBits)
        {
            this.tableIndexSizeBits = tableSizeBits;
            this.mSizeBits = mSizeBits;

            // Number of tables is celing(UniversumBits / TableSize)
            int numberOfTables = (64 / tableSizeBits) + (64 % tableSizeBits == 0 ? 0 : 1);
            tables = new ulong[numberOfTables][];

            for (int i = 0; i < numberOfTables; i++)
            {
                tables[i] = new ulong[1 << tableSizeBits];
            }

            Reset();
        }

        public ulong Hash(ulong input)
        {
            ulong mask = (ulong)(1 << tableIndexSizeBits) - 1;
            Debug.Assert(mask <= int.MaxValue);

            int index = (int)(input & mask);
            ulong result = tables[0][index];

            for (int i = 1; i < tables.Length; i++)
            {
                Debug.Assert(mask <= int.MaxValue);

                mask = mask << tableIndexSizeBits;
                index = (int)(input & mask);

                Debug.Assert((ulong)index < (1ul << tableIndexSizeBits));
                result = result ^ tables[i][index];
            }

            Debug.Assert(result < (1ul << mSizeBits)); // Assume mSizeBits < 64, otherwise this assert will fire everytime

            return result;
        }

        private void Reset()
        {

            for (int i = 0; i < tables.Length; i++)
            {
                for (int j = 0; j < tables[i].Length; j++)
                {
                    tables[i][j] = (ulong)rnd.Next(1 << mSizeBits);

                }
            }
            throw new NotImplementedException();
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
