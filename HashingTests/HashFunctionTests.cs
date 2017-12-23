using System;
using System.Collections.Generic;
using Xunit;
using Hashing;

namespace HashingTests
{
    public class HashFunctionTests
    {
        [Fact]
        public void NaiveModuloHashLinTest()
        {
            LinearHashTable<ulong> myHashTable = new LinearHashTable<ulong>(10, new NaiveModuloHash(10));
            TestFirstAndLastNElements(myHashTable, 300);
        }

        [Fact]
        public void TableHashLinTest()
        {
            LinearHashTable<ulong> myHashTable = new LinearHashTable<ulong>(10, new TableHash(10, 16));
            TestFirstAndLastNElements(myHashTable, 300);
        }

        [Fact]
        public void MultiShiftHashLinTest()
        {
            LinearHashTable<ulong> myHashTable = new LinearHashTable<ulong>(10, new MultiShiftHash(10));
            TestFirstAndLastNElements(myHashTable, 300);
        }

        [Fact]
        public void MultiShiftHashCuckooTest()
        {
            var myHashTable = new CuckooHashTable<ulong>(10, new MultiShiftHash(10), new MultiShiftHash(10));
            TestFirstAndLastNElements(myHashTable, 300);
        }

        [Fact]
        public void TableHashCuckooTest()
        {
            var myHashTable = new CuckooHashTable<ulong>(10, new TableHash(10, 16), new TableHash(10, 16));
            TestFirstAndLastNElements(myHashTable, 300);
        }

        private static void TestFirstAndLastNElements(IHashTable<ulong, ulong> myHashTable, ulong numberOfElements)
        {
            for (ulong i = 0; i < numberOfElements; i++)
            {
                ulong otherI = (ulong)ulong.MaxValue - i;

                myHashTable.Add(i + 1, i + 1);
                myHashTable.Add(otherI, otherI);
            }

            for (ulong i = 0; i < numberOfElements; i++)
            {
                ulong otherI = (ulong)ulong.MaxValue - i;

                Assert.Equal(i + 1, myHashTable.Find(i + 1));
                Assert.Equal(otherI, myHashTable.Find(otherI));
            }
        }
    }
}
