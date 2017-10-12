using System;

namespace SplayTree
{
    class Program
    {
        static void Main(string[] args)
        {
            SplayNode splayTree = null;

            string line;
            while((line = Console.ReadLine()) != null)
            {
                (int number, char command) = ParseNumber(line);

                switch (command)
                {
                    // new tree of size `number`
                    case '#':
                        splayTree = null;
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
