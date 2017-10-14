using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SplayTree
{
    class SplayTree
    {
        SplayNode Root { get; set; }

        public void Insert(int key)
        {
            var newNode = new SplayNode(key);

            if (Root == null) { Root = newNode; return; }
            var currNode = Root;

            while (true)
            {
                int currKey = currNode.Key;
                int newKey = newNode.Key;

                if (newKey == currKey) { return; }
                else if (newKey > currKey)
                {
                    if (currNode.RightSon == null) { currNode.RightSon = newNode; return; }
                    else { currNode = currNode.RightSon; continue; }
                }
                else if (newKey < currKey)
                {
                    if (currNode.LeftSon == null) { currNode.LeftSon = newNode; return; }
                    else { currNode = currNode.LeftSon; continue; }
                }
            }
        }

        public void Find(int key)
        {
            var currNode = Root;
            int length = -1;

            while (true)
            {
                length++;
                int currKey = currNode.Key;

                if (key == currKey) { Console.WriteLine($"S: {length}"); return; }
                else if (key > currKey)
                {
                    if (currNode.RightSon == null) { Console.WriteLine($"F: {length}"); return; }
                    else { currNode = currNode.RightSon; continue; }
                }
                else if (key < currKey)
                {
                    if (currNode.LeftSon == null) { Console.WriteLine($"F: {length}"); return; }
                    else { currNode = currNode.LeftSon; continue; }
                }

            }
        }
    }

    [DebuggerDisplay("K:{Key}|L:{LeftSon?.Key}|R:{RightSon?.Key}")]
    class SplayNode
    {
        public SplayNode(int key)
        {
            Key = key;
        }

        public int Key { get; private set; }

        public SplayNode LeftSon;
        public SplayNode RightSon;

    }
}
