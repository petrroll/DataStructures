using System;

namespace SplayTree
{
    class Program
    {
        static void Main(string[] args)
        {
            SplayTree splayTree = null;
            Logger logger = new Logger();

            string line;
            while((line = Console.ReadLine()) != null)
            {
                (int number, char command) = ParseNumber(line);

                switch (command)
                {
                    case '#':
                        splayTree = new SplayTree() { Logger = logger } ;
                        GC.Collect();


                        logger.Flush();
                        logger.Initialize(number);
                        break;
                    case 'I':
                        splayTree.Insert(number);
                        break;
                    case 'F':
                        splayTree.Find(number);
                        break;
                    default:
                        throw new NotImplementedException();
                }

            }
            logger.Flush();

        }

        private static (int number, char command) ParseNumber(string line)
        {
            int number = 0;
            for (int i = 2; i < line.Length; i++, number *= 10)
            {
                number += (line[i] - '0');
            }

            number /= 10;
            return (number, line[0]);
        }
    }
}
