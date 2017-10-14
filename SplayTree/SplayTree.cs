#define SIMPLESPLAY

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
                    if (currNode.RightSon == null) { currNode.RightSon = newNode; newNode.Parent = currNode; Splay(currNode); return; }
                    else { currNode = currNode.RightSon; continue; }
                }
                else if (newKey < currKey)
                {
                    if (currNode.LeftSon == null) { currNode.LeftSon = newNode; newNode.Parent = currNode; Splay(currNode); return; }
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

                if (key == currKey) { Console.WriteLine($"S {length}"); Splay(currNode); return; }
                else if (key > currKey)
                {
                    if (currNode.RightSon == null) { Console.WriteLine($"F {length}"); Splay(currNode); return; }
                    else { currNode = currNode.RightSon; continue; }
                }
                else if (key < currKey)
                {
                    if (currNode.LeftSon == null) { Console.WriteLine($"F {length}"); Splay(currNode); return; }
                    else { currNode = currNode.LeftSon; continue; }
                }

            }
        }

        private void Splay(SplayNode node)
        {
            while(node.Parent != null)
            {
#if SIMPLESPLAY
                if(node.Parent == Root)
#endif
                {
                    if (Root.LeftSon == node) {  L(node); }
                    else { R(node); }
                }
            }
            Root = node;
        }

        private void L(SplayNode node)
        {
            var originalParent = node.Parent;
            ConnectNewParent(node, originalParent);

            originalParent.LeftSon = node.RightSon;
            if (node.RightSon != null) { node.RightSon.Parent = originalParent; }

            originalParent.Parent = node;
            node.RightSon = originalParent;
        }

        private void R(SplayNode node)
        {
            var originalParent = node.Parent;
            ConnectNewParent(node, originalParent);

            originalParent.RightSon = node.LeftSon;
            if (node.LeftSon != null) { node.LeftSon.Parent = originalParent; }

            originalParent.Parent = node;
            node.LeftSon = originalParent;

        }

        private static void ConnectNewParent(SplayNode node, SplayNode originalParent)
        {
            node.Parent = originalParent.Parent;
            if (originalParent.Parent != null)
            {
                if (originalParent.Parent.LeftSon == originalParent) { originalParent.Parent.LeftSon = node; }
                else { originalParent.Parent.RightSon = node; }
            }
        }
    }

    [DebuggerDisplay("K:{Key}|L:{LeftSon?.Key}|R:{RightSon?.Key}|P:{Parent?.Key}")]
    class SplayNode
    {
        public SplayNode(int key)
        {
            Key = key;
        }

        public int Key { get; private set; }

        public SplayNode LeftSon;
        public SplayNode RightSon;
        public SplayNode Parent; 

    }
}
