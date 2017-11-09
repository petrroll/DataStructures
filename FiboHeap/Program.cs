using System;
using System.Diagnostics;
using FibHeap;
using System.Linq;

namespace FiboHeap
{
    class Program
    {
        enum Command { New, INS, DEL, DEC}
        static FibNode<int>[] NodeIndetifierResolver;

        static void Main(string[] args)
        {
            FibHeap<int> fibHeap = null;
            Logger logger = new Logger();

            string line;
            while ((line = Console.ReadLine()) != null)
            {
                (Command command, int N, int K) = ParseCommand(line);
                switch (command)
                {
                    // Create new FiboHeap and report the last one
                    case Command.New:
                        fibHeap = null;
                        NodeIndetifierResolver = null;

                        logger.Flush();
                        GC.Collect();

                        NodeIndetifierResolver = new FibNode<int>[2_000_000];
                        fibHeap = new FibHeap<int>(logger);
                        logger.Initialize(N);

                        Debug.Assert(K == -1);
                        break;
                    case Command.INS:
                        NodeIndetifierResolver[N] = fibHeap.Insert(N, K);
                        break;
                    case Command.DEL:
                        var identifier = fibHeap.Delete();
                        Debug.Assert(NodeIndetifierResolver.Min(node => (node != null) ? node.Weight : int.MaxValue) == identifier.Weight);
                        NodeIndetifierResolver[identifier.Value] = null;

                        Debug.Assert(N == -1 && K == -1);
                        break;
                    case Command.DEC:
                        var nodeToDecrease = NodeIndetifierResolver[N];
                        if (nodeToDecrease != null) { fibHeap.Decrease(nodeToDecrease, K); }
                        break;
                    default:
                        throw new NotImplementedException();
                }

            }
            // Report the last tree
            logger.Flush();
        }

        private static (Command command, int N, int K) ParseCommand(string line)
        {
            int indexInInput = 2;
            var command = Command.New;

            if (line[0] != '#')
            {
                if (line[0] == 'I') { command = Command.INS; }
                else if (line[2] == 'L') { command = Command.DEL; }
                else { command = Command.DEC; }

                indexInInput = 3;
            }
            
            // DEL: No number arguments
            if (command == Command.DEL) { return (command, -1, -1); }

            // Either New (one number orgument) or DEC / INS (two number arguments)
            int N = parseNumber(line, ref indexInInput);
            int K = (command == Command.New) ? -1 : parseNumber(line, ref indexInInput);

            return (command, N, K);


            int parseNumber(string input, ref int indexInString)
            {
                while (indexInString < input.Length && input[indexInString] == ' ') { indexInString++; }

                int result = 0;
                while (indexInString < input.Length && input[indexInString] != ' ')
                {
                    result *= 10;
                    result += (input[indexInString] - '0');
                    indexInInput++;
                }

                return result;
            }
        }


    }
}
