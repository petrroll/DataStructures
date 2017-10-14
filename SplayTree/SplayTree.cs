//#define SIMPLESPLAY

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
            ConnectNewRoot(node, originalParent);

            SetAsLeftSon(originalParent, node.RightSon);
            SetAsRightSonNN(node, originalParent);
        }

        private void R(SplayNode node, SplayNode parent)
        {
            var originalParent = parent;
            ConnectNewRoot(node, originalParent);

            SetAsRightSon(originalParent, node.LeftSon);
            SetAsLeftSonNN(node, originalParent);

        }

        private void LL(SplayNode node, SplayNode parent, SplayNode grandParent)
        {
            var B = node.RightSon;
            var C = parent.RightSon;
            var D = grandParent.RightSon;

            SetAsLeftSon(parent, B);
            SetAsLeftSon(grandParent, C);
            SetAsRightSon(grandParent, D);

            ConnectNewRoot(node, grandParent);
            SetAsRightSonNN(parent, grandParent);
            SetAsRightSonNN(node, parent);
        }

        private void LR(SplayNode node, SplayNode parent, SplayNode grandParent)
        {
            var B = node.LeftSon;
            var C = node.RightSon;
            var D = grandParent.RightSon;

            SetAsRightSon(parent, B);
            SetAsLeftSon(grandParent, C);
            SetAsRightSon(grandParent, D);

            ConnectNewRoot(node, grandParent);
            SetAsLeftSonNN(node, parent);
            SetAsRightSonNN(node, grandParent);

        }

        private void RR(SplayNode node, SplayNode parent, SplayNode grandParent)
        {
            var B = node.LeftSon;
            var C = parent.LeftSon;
            var D = grandParent.LeftSon;

            SetAsRightSon(parent, B);
            SetAsRightSon(grandParent, C);
            SetAsRightSon(grandParent, D);

            ConnectNewRoot(node, grandParent);
            SetAsLeftSonNN(parent, grandParent);
            SetAsLeftSonNN(node, parent);
        }

        private void RL(SplayNode node, SplayNode parent, SplayNode grandParent)
        {
            var B = node.RightSon;
            var C = node.LeftSon;
            var D = grandParent.LeftSon;

            SetAsLeftSon(parent, B);
            SetAsRightSon(grandParent, C);
            SetAsLeftSon(grandParent, D);

            ConnectNewRoot(node, grandParent);
            SetAsRightSonNN(node, parent);
            SetAsLeftSonNN(node, grandParent);
        }

        private static void ConnectNewRoot(SplayNode newRoot, SplayNode originalRoot)
        {
            newRoot.Parent = originalRoot.Parent;
            if (originalRoot.Parent != null)
            {
                if (originalRoot.Parent.LeftSon == originalRoot) { originalRoot.Parent.LeftSon = newRoot; }
                else { originalRoot.Parent.RightSon = newRoot; }
            }
        }

        private static void SetAsLeftSon(SplayNode node, SplayNode newLeftSon)
        {
            node.LeftSon = newLeftSon;
            if(newLeftSon != null)
            {
                newLeftSon.Parent = node;
            }
        }

        private static void SetAsRightSon(SplayNode node, SplayNode newRightSon)
        {
            node.RightSon = newRightSon;
            if (newRightSon != null)
            {
                newRightSon.Parent = node;
            }
        }


        private static void SetAsLeftSonNN(SplayNode node, SplayNode newLeftSon)
        {
            node.LeftSon = newLeftSon;
            newLeftSon.Parent = node;
        }

        private static void SetAsRightSonNN(SplayNode node, SplayNode newRightSon)
        {
            node.RightSon = newRightSon;
            newRightSon.Parent = node;  
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
