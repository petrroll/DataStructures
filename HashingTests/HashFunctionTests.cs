using System;
using System.Collections.Generic;
using Xunit;
using Hashing;

namespace HashingTests
{
    public class HashFunctionTests
    {
        [Fact]
        public void NaiveModuloHashTest()
        {
            LinearHashTable<ulong> myHashTable = new LinearHashTable<ulong>(10, new NaiveModuloHash(10));

            for (ulong i = 0; i < 100; i++)
            {
                ulong otherI = (ulong)ulong.MaxValue - i;

                myHashTable.Add(i, i);
                myHashTable.Add(otherI, otherI);
            }

            for (ulong i = 0; i < 100; i++)
            {
                ulong otherI = (ulong)ulong.MaxValue - i;

                Assert.Equal(i, myHashTable.Find(i));
                Assert.Equal(otherI, myHashTable.Find(otherI));
            }
        }

        [Fact]
        public void TableHashTest()
        {
            LinearHashTable<ulong> myHashTable = new LinearHashTable<ulong>(10, new TableHash(10, 16));

            for (ulong i = 0; i < 100; i++)
            {
                ulong otherI = (ulong)ulong.MaxValue - i;

                myHashTable.Add(i, i);
                myHashTable.Add(otherI, otherI);
            }

            for (ulong i = 0; i < 100; i++)
            {
                ulong otherI = (ulong)ulong.MaxValue - i;

                Assert.Equal(i, myHashTable.Find(i));
                Assert.Equal(otherI, myHashTable.Find(otherI));
            }
        }

        [Fact]
        public void MultiShiftHashTest()
        {
            LinearHashTable<ulong> myHashTable = new LinearHashTable<ulong>(10, new MultiShiftHash(10));

            for (ulong i = 0; i < 100; i++)
            {
                ulong otherI = (ulong)ulong.MaxValue - i;

                myHashTable.Add(i, i);
                myHashTable.Add(otherI, otherI);
            }

            for (ulong i = 0; i < 100; i++)
            {
                ulong otherI = (ulong)ulong.MaxValue - i;

                Assert.Equal(i, myHashTable.Find(i));
                Assert.Equal(otherI, myHashTable.Find(otherI));
            }
        }
    }
}
