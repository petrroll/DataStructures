// Uncomment and recompile to create a version that does single rotations only.
// #define SIMPLESPLAY

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;


namespace SplayTree
{
    class SplayTree
    {
        Logger logger;
        SplayNode root;

        public SplayTree(Logger logger)
        {
            this.logger = logger;
        }

        public void Insert(int key)
        {
            var newNode = new SplayNode(key);

            if (root == null) { root = newNode; return; }
            var currNode = root;

            // DFS: insert, and splay the parent of the new node.
            while (true)
            {
                int currKey = currNode.Key;
                int newKey = newNode.Key;

                if (newKey == currKey) { Splay(currNode); return; }
                else if (newKey > currKey)
                {
                    if (currNode.RightSon == null) { SetAsRightSonNotNull(currNode, newNode); Splay(currNode); return; }
                    else { currNode = currNode.RightSon; continue; }
                }
                else if (newKey < currKey)
                {
                    if (currNode.LeftSon == null) { SetAsLeftSonNotNull(currNode, newNode); Splay(currNode); return; }
                    else { currNode = currNode.LeftSon; continue; }
                }
            }
        }

        public void Find(int key)
        {
            if (root == null) { throw new InvalidOperationException("Can't call Find on an empty tree."); }

            var currNode = root;
            int length = -1;

            // DFS: keep the current distance from root, report it to logger once the node is found / not found, splay the last visited node.
            while (true)
            {
                length++;
                int currKey = currNode.Key;

                if (key == currKey) { logger.AddValue(length); Splay(currNode); return; }
                else if (key > currKey)
                {
                    if (currNode.RightSon == null) { logger.AddValue(length); Splay(currNode); return; }
                    else { currNode = currNode.RightSon; continue; }
                }
                else if (key < currKey)
                {
                    if (currNode.LeftSon == null) { logger.AddValue(length); Splay(currNode); return; }
                    else { currNode = currNode.LeftSon; continue; }
                }

            }
        }

        private void Splay(SplayNode node)
        {
            SplayNode parent;
            while((parent = node.Parent) != null)
            {
#if !SIMPLESPLAY
                // Use simple rotations only on the topmost level (unless SIMPLESPLAY is set, if it is use them exclusively).
                if(parent == root)
#endif
                {
                    if (parent.LeftSon == node) {  L(node, parent); }
                    else { R(node, parent); }

                    continue;
                }

#if !SIMPLESPLAY
                // Double rotations
                var grandParent = parent.Parent;
                if(grandParent.LeftSon == parent)
                {
                    if (parent.LeftSon == node)
                    {
                        LL(node, parent, grandParent);
                    }
                    else
                    {
                        LR(node, parent, grandParent);
                    }
                }
                else
                {
                    if (parent.LeftSon == node)
                    {
                        RL(node, parent, grandParent); 
                    }
                    else
                    {
                        RR(node, parent, grandParent);
                    }
                }
#endif

            }

            // 
            root = node;
        }

        private void L(SplayNode node, SplayNode parent)
        {
            var originalParent = parent;
            ConnectNewRoot(node, originalParent);

            SetAsLeftSon(originalParent, node.RightSon);
            SetAsRightSonNotNull(node, originalParent);
        }

        private void R(SplayNode node, SplayNode parent)
        {
            var originalParent = parent;
            ConnectNewRoot(node, originalParent);

            SetAsRightSon(originalParent, node.LeftSon);
            SetAsLeftSonNotNull(node, originalParent);

        }

        private void LL(SplayNode node, SplayNode parent, SplayNode grandParent)
        {
            var B = node.RightSon;
            var C = parent.RightSon;

            SetAsLeftSon(parent, B);
            SetAsLeftSon(grandParent, C);

            ConnectNewRoot(node, grandParent);
            SetAsRightSonNotNull(parent, grandParent);
            SetAsRightSonNotNull(node, parent);
        }

        private void LR(SplayNode node, SplayNode parent, SplayNode grandParent)
        {
            var B = node.LeftSon;
            var C = node.RightSon;

            SetAsRightSon(parent, B);
            SetAsLeftSon(grandParent, C);

            ConnectNewRoot(node, grandParent);
            SetAsLeftSonNotNull(node, parent);
            SetAsRightSonNotNull(node, grandParent);
        }

        private void RR(SplayNode node, SplayNode parent, SplayNode grandParent)
        {
            var B = parent.LeftSon;
            var C = node.LeftSon;

            SetAsRightSon(parent, C);
            SetAsRightSon(grandParent, B);

            ConnectNewRoot(node, grandParent);
            SetAsLeftSonNotNull(parent, grandParent);
            SetAsLeftSonNotNull(node, parent);
        }

        private void RL(SplayNode node, SplayNode parent, SplayNode grandParent)
        {
            var B = node.LeftSon;
            var C = node.RightSon;

            SetAsRightSon(grandParent, B);
            SetAsLeftSon(parent, C);

            ConnectNewRoot(node, grandParent);
            SetAsRightSonNotNull(node, parent);
            SetAsLeftSonNotNull(node, grandParent);
        }

        // Not NotNull helper methods are identical to those above but do not include one (in certain situations redundant) 
        // null check. While the gain of this is minimal it is on (very) hot path and as such was deemed worth it. 

        // [MethodImpl(MethodImplOptions.AggressiveInlining)] should also not be necessary since JIT is usually clever enough
        // to inline methods as simple as these. Since they are on a very hot path, however, it's best to hint JIT that we really 
        // want to inline them.

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ConnectNewRoot(SplayNode newRoot, SplayNode originalRoot)
        {
            newRoot.Parent = originalRoot.Parent;
            if (originalRoot.Parent != null)
            {
                if (originalRoot.Parent.LeftSon == originalRoot) { originalRoot.Parent.LeftSon = newRoot; }
                else { originalRoot.Parent.RightSon = newRoot; }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SetAsLeftSon(SplayNode node, SplayNode newLeftSon)
        {
            node.LeftSon = newLeftSon;
            if(newLeftSon != null)
            {
                newLeftSon.Parent = node;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SetAsRightSon(SplayNode node, SplayNode newRightSon)
        {
            node.RightSon = newRightSon;
            if (newRightSon != null)
            {
                newRightSon.Parent = node;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SetAsLeftSonNotNull(SplayNode node, SplayNode newLeftSon)
        {
            node.LeftSon = newLeftSon;
            newLeftSon.Parent = node;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SetAsRightSonNotNull(SplayNode node, SplayNode newRightSon)
        {
            node.RightSon = newRightSon;
            newRightSon.Parent = node;  
        }
    }

    [DebuggerDisplay("K:{Key}|L:{LeftSon?.Key}|R:{RightSon?.Key}|P:{Parent?.Key}")]
    class SplayNode
    {

        // In a real splay tree implementation the node would contain either a generic "Value" property 
        // or the Key would be a generic T type implementing a IComperable interface so that the tree can
        // actually serve as more than a fancy benchmark. Either the key or the value property would, then,
        // get returned in the Find function (that, in this version doesn't return anything but just records
        // the traveled path to the Logger.

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
