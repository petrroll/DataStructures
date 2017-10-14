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
            SplayNode parent;
            while((parent = node.Parent) != null)
            {
#if SIMPLESPLAY
                if(parent == Root)
#endif
                {
                    if (parent.LeftSon == node) {  L(node, parent); }
                    else { R(node, parent); }

                    continue;
                }

                var grandParent = parent.Parent;
                if(grandParent.LeftSon == parent)
                {
                    if (parent.LeftSon == node) { LL(node, parent, grandParent); }
                    else { LR(node, parent, grandParent); }
                }
                else
                {
                    if (parent.LeftSon == node) { RL(node, parent, grandParent); }
                    else { RR(node, parent, grandParent); }
                }
            }
            Root = node;
        }

        private void L(SplayNode node, SplayNode parent)
        {
            var originalParent = parent;
            ConnectNewParent(node, originalParent);

            originalParent.LeftSon = node.RightSon;
            if (node.RightSon != null) { node.RightSon.Parent = originalParent; }

            originalParent.Parent = node;
            node.RightSon = originalParent;
        }

        private void R(SplayNode node, SplayNode parent)
        {
            var originalParent = parent;
            ConnectNewParent(node, originalParent);

            originalParent.RightSon = node.LeftSon;
            if (node.LeftSon != null) { node.LeftSon.Parent = originalParent; }

            originalParent.Parent = node;
            node.LeftSon = originalParent;

        }

        private void LL(SplayNode node, SplayNode parent, SplayNode grandParent)
        {

        }

        private void LR(SplayNode node, SplayNode parent, SplayNode grandParent)
        {

        }

        private void RR(SplayNode node, SplayNode parent, SplayNode grandParent)
        {

        }

        private void RL(SplayNode node, SplayNode parent, SplayNode grandParent)
        {

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
