using System;
using System.Diagnostics;
using FibHeap;
using System.Linq;

namespace FiboHeap
{
    class Program
    {
        enum Command { New, INS, DEL, DEC}

        static void Main(string[] args)
        {
            // To function fast the alghoritm needs references to the nodes for the Decrease operation instead of
            // ... just keys. The generator, on the other hand, does not (nor it can) operate with references. A 
            // ... decision to put the translation between references and identifiers outside of the heap was made
            // ... as in a real world application the outside code would use the references instead of identifiers for
            // ... decrease anyway. 
            FibNode<int>[] NodeIndetifierResolver = null;

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
                        // Another slow Debug.Assert that checks a truly minimal node has been returned (it could be made way faster easily, but it's just a debug.assert code)
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

        static (Command command, int N, int K) ParseCommand(string line)
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

            // The reason for a custom number parsing is speed. While there're build in `int.Parse` / `int.TryParse` they need to accomodate 
            // ...various number formats and as such are not as fast. And with > 10 % of runtime spend in input processing every speed increase
            // ...is worth it (the 10 % was measured in last excercise, used the same logic here because it was quicker to setup). 
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
