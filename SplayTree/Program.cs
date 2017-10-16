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
                (int number, char command) = ParseCommand(line);

                switch (command)
                {
                    // Create new SplayTree and report the last one
                    case '#':
                        splayTree = null;
                        logger.Flush();
                        GC.Collect();

                        splayTree = new SplayTree(logger);
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
            // Report the last tree
            logger.Flush();

        }

        private static (int number, char command) ParseCommand(string line)
        {
            char command = line[0];

            int number = 0;
            for (int i = 2; i < line.Length; i++, number *= 10)
            {
                number += (line[i] - '0');
            }
            number /= 10;

            return (number, command);
        }
    }
}
