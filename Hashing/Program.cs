using System;
using System.IO;

namespace Hashing
{
    class Program
    {
        public static object Writer { get; private set; }

        static void Main(string[] args)
        {
            firstAssigment();
            secondAssigment();
        }

        private static void firstAssigment()
        {
            int hashTableSizeBits = 24;
            ulong[] randNumbers = initRandomNumbers(hashTableSizeBits);

            testSteps(hashTableSizeBits, randNumbers);
            testTime(hashTableSizeBits, randNumbers);
        }

        private static void secondAssigment()
        {
            AdvancedStepsLogger stepsLogger = new AdvancedStepsLogger();

            (Func<int, IHashTable<bool, ulong>> tableFactory, string name)[] config = {
                ((int tableSizeInBits) => new LinearHashTable<bool>(tableSizeInBits, new MultiShiftHash(tableSizeInBits)), "linMultiShift"),
                ((int tableSizeInBits) => new LinearHashTable<bool>(tableSizeInBits, new TableHash(tableSizeInBits, 16)), "linTable")
            };


            foreach(var testCase in config)
            {
                Console.WriteLine($"Started {testCase.name}");
                statisticallyTestOneHashConfig(stepsLogger, testCase.tableFactory, testCase.name);
            }
        }

        private static void statisticallyTestOneHashConfig(AdvancedStepsLogger stepsLogger, Func<int, IHashTable<bool, ulong>> hashTableCreator, string name)
        {
            using (stepsLogger.wrMin = new StreamWriter($"data/min_{name}.out"))
            using (stepsLogger.wrMax = new StreamWriter($"data/max_{name}.out"))
            using (stepsLogger.wrAvg = new StreamWriter($"data/avg_{name}.out"))
            using (stepsLogger.wrMed = new StreamWriter($"data/med_{name}.out"))
            using (stepsLogger.wrDec = new StreamWriter($"data/dec_{name}.out"))
            {
                for (int hashTableSizeBits = 5; hashTableSizeBits < 24; hashTableSizeBits++)
                {
                    Console.WriteLine($"Run for size {hashTableSizeBits}");

                    var hashTable = hashTableCreator(hashTableSizeBits);
                    hashTable.Logger = stepsLogger;

                    stepsLogger.InitNewForOneSize(hashTableSizeBits);
                    for (int k = 0; k < 100; k++)
                    {
                        ulong i = 1;
                        for (; i < 0.89 * (1 << hashTableSizeBits); i++)
                        {
                            hashTable.Add(i, true);
                        }

                        stepsLogger.StartNewSegment();
                        for (; i < 0.91 * (1 << hashTableSizeBits); i++)
                        {

                            hashTable.Add(i, true);
                            stepsLogger.NewElementBoundary();
                        }
                        stepsLogger.FlushElementSegment();
                        hashTable.Reset();
                    }
                    stepsLogger.PrintStatisticalSummaryForOneSize();
                }
            }
        }

        private static void testSteps(int hashTableSizeBits, ulong[] randNumbers)
        {
            var config = getCongigs(hashTableSizeBits);
            StepsLogger stepsLogger = new StepsLogger();

            foreach (var testCase in config)
            {
                Console.WriteLine($"{testCase.name} steps.");
                benchmarkHashTable(stepsLogger, hashTableSizeBits, randNumbers, testCase.hashTable, testCase.name, testCase.maxFillFactor);
            }
        }


        private static void testTime(int hashTableSizeBits, ulong[] randNumbers)
        {
            var config = getCongigs(hashTableSizeBits);
            TimeLogger timeLogger = new TimeLogger();

            foreach (var testCase in config)
            {
                Console.WriteLine($"{testCase.name} time.");
                benchmarkHashTableTime(timeLogger, hashTableSizeBits, randNumbers, testCase.hashTable, testCase.name, testCase.maxFillFactor);

            }
        }

        private static (string name, IHashTable<bool, ulong> hashTable, double maxFillFactor)[] getCongigs(int hashTableSizeBits)
        {
            (string name, IHashTable<bool, ulong> hashTable, double maxFillFactor)[] config = {
                ("linNaive", new LinearHashTable<bool>(hashTableSizeBits, new NaiveModuloHash(hashTableSizeBits)), 0.95),
                ("linMulti", new LinearHashTable<bool>(hashTableSizeBits, new MultiShiftHash(hashTableSizeBits)), 0.95),
                ("linTable", new LinearHashTable<bool>(hashTableSizeBits, new TableHash(hashTableSizeBits, 8)), 0.95),

                ("cuckTable", new CuckooHashTable<bool>(hashTableSizeBits, new TableHash(hashTableSizeBits, 16), new TableHash(hashTableSizeBits, 16)), 0.49),
                ("cuckMulti", new CuckooHashTable<bool>(hashTableSizeBits, new MultiShiftHash(hashTableSizeBits), new MultiShiftHash(hashTableSizeBits)), 0.49)
            };

            return config;
        }

        private static void benchmarkHashTableTime(TimeLogger logger, int hashTableSizeBits, ulong[] randNumbers, IHashTable<bool, ulong> myHashTable, string filename, double maxFillFactor)
        {

            using (logger.Writer = new StreamWriter($"data/{filename}_time.out"))
            {
                GC.Collect();
                for (int i = 0; i < randNumbers.Length * maxFillFactor; i++)
                {
                    logger.StartElementSegment();
                    for (int j = 0; j < randNumbers.Length * maxFillFactor / 100 && i < randNumbers.Length * maxFillFactor; i++, j++)
                    {
                        myHashTable.Add(randNumbers[i], true);
                        logger.NewElementBoundary();
                    }
                    logger.FlushElementSegment(i / (double)(1 << hashTableSizeBits));
                }
            }
        }

        private static void benchmarkHashTable(StepsLogger logger, int hashTableSizeBits, ulong[] randNumbers, IHashTable<bool, ulong> myHashTable, string filename, double maxFillFactor)
        {
            myHashTable.Logger = logger;
            using (logger.Writer = new StreamWriter($"data/{filename}.out"))
            {
                for (int i = 0; i < randNumbers.Length * maxFillFactor; i++)
                {
                    for (int j = 0; j < randNumbers.Length * maxFillFactor / 100 && i < randNumbers.Length * maxFillFactor; i++, j++)
                    {
                        myHashTable.Add(randNumbers[i], true);
                        logger.NewElementBoundary();
                    }
                    logger.FlushElementSegment(i / (double)(1 << hashTableSizeBits));
                }
            }
        }

        private static ulong[] initRandomNumbers(int hashTableSizeBits)
        {
            Random rnd = new Random();
            ulong[] randNumbers = new ulong[1 << hashTableSizeBits];
            for (int i = 0; i < randNumbers.Length; i++)
            {
                randNumbers[i] = rnd.NextULong();
            }

            return randNumbers;
        }
    }

    
}
