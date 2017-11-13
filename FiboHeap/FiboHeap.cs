//#define SIMPLE

using FiboHeap;
using System;
using System.Diagnostics;

namespace FibHeap
{
    // 
    // The class is full of Debug.Assert some of which are incredibely slow. While I considered deleting them
    // ... since they don't add anything to the code at first sight they show some invariants I believe should
    // ... be true about the code and points I considered potentially dangerous for bug creep. Do not run 
    // ... in DEBUG mode though, it's significantly slower.
    //


    public class FibHeap<T>
    {
        CyclicForrest<T> root;
        FibNode<T> currMinimum;

        int numberOfElements;
        Logger logger;

        public FibHeap(Logger logger)
        {
            this.logger = logger;
        }

        public FibNode<T> Insert(T payload, int weight)
        {
            Debug.Assert(root.CheckChildren());
            var newNode = new FibNode<T>(payload, weight);

            UpdateMinimum(newNode);
            numberOfElements++;

            // Add the new node as a stanadone tree to the current heap forrest.
            root.AddSingleNode(newNode);

            return newNode;
        }

        public FibNode<T> Delete()
        {
            Debug.Assert(root.CheckChildren());
            logger.AddNumberOfOperations();

            Debug.Assert(currMinimum != null || numberOfElements == 0);
            if(numberOfElements == 0) { return null; }

            var minimum = currMinimum;
            currMinimum = null;

            // Remove minimum from the current heap forrest.
            root.RemoveChild(minimum);
            numberOfElements--;

            Debug.Assert(numberOfElements >= 0);
            Debug.Assert(numberOfElements > 1 || root.IsEmpty);

            logger.AddValue(minimum.Children.Rank);

            // Add minimum's children to the current heap forrest.
            root.AddChildren(minimum.Children);
            Consolidate();

            Debug.Assert(root.CheckChildren());
            return minimum;
        }

        public void Decrease(FibNode<T> payload, int newWeigth)
        {
            Debug.Assert(newWeigth <= payload.Weight);

            payload.Weight = newWeigth;
            UpdateMinimum(payload);

            if (payload.Parent == null) { return; }
            if (payload.Parent.Weight < payload.Weight) { return; }

            Cut(payload);
        }

        private void Cut(FibNode<T> payload)
        {
            var parent = payload.Parent;

            // Cut the current node from it's parent and add it to the current heap forrest.
            parent.Children.RemoveChild(payload);
            root.AddSingleNode(payload);

            #if !SIMPLE // If not simple, continue with marking & cuting 
            payload.Marked = false;

            if (parent.Marked) { Cut(parent); }
            else if (parent.Parent != null) { parent.Marked = true; }
            #endif
        }

        private void Consolidate()
        {
            if (root.IsEmpty)
            {
                Debug.Assert(numberOfElements == 0);
                return;
            }

            // The maximum rank is log2(numberOfElements). +1 because the array is 0-based.  
            var n = (int)Math.Ceiling(Math.Log(numberOfElements, 2)) + 1;
            FibNode<T>[] trees = new FibNode<T>[n];

            // Create trees for a new heap forrest from the old heap forrest
            {
                var currTree = root.FirstChild;
                do
                {
                    var nextNode = currTree.NextSibling;
                    ProcessTree(trees, currTree);
                    currTree = nextNode;

                } while (currTree != root.FirstChild);
            }

            // Create new heap forrest from the newly created trees with unique ranks
            {
                uint newHeapRank = 0;
                FibNode<T> newHeapRoot = null;
                FibNode<T> lastNewHeapNode = null;

                for (int i = 0; i < trees.Length; i++)
                {
                    FibNode<T> newTree = trees[i];
                    if (newTree == null) { continue; }

                    AddTreeToNewHeap(ref newHeapRank, ref newHeapRoot, ref lastNewHeapNode, newTree);
                }

                lastNewHeapNode.NextSibling = newHeapRoot;
                newHeapRoot.PreviousSibling = lastNewHeapNode;

                root = new CyclicForrest<T>(newHeapRoot, newHeapRank);
            }

            Debug.Assert(root.CheckChildren());
            
            //
            // Below are only local functions. They are not inlined because I believe it's more readable this way since they all
            // .. represent a standalone functionality. Others might disagree with that assesment, however.
            //

            // Connects a new tree to a heap forrest that is just being build
            void AddTreeToNewHeap(ref uint rank, ref FibNode<T> firstNode, ref FibNode<T> lastNode, FibNode<T> tree)
            {
                rank++;

                tree.Parent = null;
                tree.Marked = false;

                UpdateMinimum(tree);

                if (firstNode == null) { firstNode = tree; }
                else
                {
                    lastNode.NextSibling = tree;
                    tree.PreviousSibling = lastNode;
                }

                lastNode = tree;
            }

            // Takes a tree and tries to add it to a rank-indexed tree array. If there is more than 
            // ...one tree with the same rank it merges them and processes the newly created tree.
            // While recursion might seeem as a problem it can't be deeper than the maximum rank possible (with each
            // ...level the rank of currently processed tree increases by one) which is log2(number of elements).
            void ProcessTree(FibNode<T>[] treesArray, FibNode<T> treeBeingAdded)
            {
                uint currRank = treeBeingAdded.Children.Rank;
                var otherTree = treesArray[currRank];

                if (otherTree == null)
                {
                    treesArray[currRank] = treeBeingAdded;
                }
                else
                {
                    // Remove the tree the currently processed one will get merged with
                    treesArray[currRank] = null; 

                    var lighterTree = treeBeingAdded.Weight < otherTree.Weight ? treeBeingAdded : otherTree;
                    var heavierTree = treeBeingAdded.Weight < otherTree.Weight ? otherTree : treeBeingAdded;

                    FibNode<T> newTree = MergeTrees(lighterTree, heavierTree);

                    Debug.Assert(currRank + 1 == newTree.Children.Rank);
                    ProcessTree(treesArray, newTree);
                }
            }

            FibNode<T> MergeTrees(FibNode<T> lighterTree, FibNode<T> heavierTree)
            {
                Debug.Assert(lighterTree.Children.Rank == heavierTree.Children.Rank);
                Debug.Assert(lighterTree.Weight <= heavierTree.Weight && heavierTree != lighterTree);

                logger.AddValue(1);
                
                // Remove the heavier tree from its original context and add it as a new sibling under the lighter tree.
                heavierTree.ResetSiblingsAndParentToATreeState();
                heavierTree.Parent = lighterTree;
                lighterTree.Children.AddSingleNode(heavierTree);

                Debug.Assert(lighterTree.Children.CheckChildren());
                Debug.Assert(!lighterTree.Children.IsEmpty);

                return lighterTree;
            }
        }

        void UpdateMinimum (FibNode<T> node)
        {
            if(currMinimum == null) { currMinimum = node; }
            if(currMinimum.Weight >= node.Weight) { currMinimum = node; }
        }

    }

    struct CyclicForrest<T>
    {
        public bool IsEmpty => (FirstChild == null);

        public FibNode<T> FirstChild;
        internal uint Rank;

        public CyclicForrest(FibNode<T> firstChild, uint rank)
        {
            FirstChild = firstChild;
            Rank = rank;
        }

        /// <summary>
        /// Connects a single FibNode<T> to the current CyclicForrest.
        /// </summary>
        internal void AddSingleNode(FibNode<T> newNode) => ConnectToCurrentCyclicList(1, newNode);

        /// <summary>
        /// Merges another CyclicForrest into the current one.
        /// </summary>
        internal void AddChildren(CyclicForrest<T> newChildren) => ConnectToCurrentCyclicList(newChildren.Rank, newChildren.FirstChild);

        void ConnectToCurrentCyclicList(uint numberOfNewChildren, FibNode<T> newChild)
        {
            // If there's nothing new to connect, return.
            if (newChild == null) { return; }

            Rank += numberOfNewChildren;

            // Current CyclicForrest is empty, set the new one as current FirstChild ref.
            if (FirstChild == null)
            {
                Debug.Assert(Rank == numberOfNewChildren);
                FirstChild = newChild;
                return;
            }

            var current = FirstChild;
            var preCurrent = current.PreviousSibling;

            var preNewChild = newChild.PreviousSibling;

            // Merge the two cyclic double linked lists.
            preCurrent.NextSibling = newChild;
            newChild.PreviousSibling = preCurrent;

            preNewChild.NextSibling = current;
            current.PreviousSibling = preNewChild;

            Debug.Assert(CheckChildren());
        }

        internal void RemoveChild(FibNode<T> nodeBeingRemoved)
        {
            Debug.Assert(Rank >= 1);
            Rank -= 1;

            var preNode = nodeBeingRemoved.PreviousSibling;
            var posNode = nodeBeingRemoved.NextSibling;

            if (Rank == 0)
            {
                // There's only one node, set the FirstChild ref to null.
                Debug.Assert(posNode == nodeBeingRemoved && preNode == nodeBeingRemoved);
                FirstChild = null;
            }
            else
            {
                // Connect the node before and after the one being removed, bypassing the one that is being removed.
                preNode.NextSibling = posNode;
                posNode.PreviousSibling = preNode;

                // Update FirstChild pointer in case the item being removed is it. There is definitely another node (see if above) available.
                if (FirstChild == nodeBeingRemoved)
                {
                    FirstChild = posNode;
                }
            }

            // Fix the removed node's pointers to siblings and to parent.
            nodeBeingRemoved.ResetSiblingsAndParentToATreeState();

            Debug.Assert(CheckChildren());
        }

        /// <summary>
        /// Checks whether the rank is in fact equal to the number of children.
        /// </summary>
        /// <remarks>
        /// It slows computation to crawl. Use only when needed.
        /// </remarks>
        internal bool CheckChildren()
        {
            if (Rank == 0) { return FirstChild == null; }

            int i = 0;
            var currTree = FirstChild;
            do
            {
                i++;
                currTree = currTree.NextSibling;
            } while (currTree != FirstChild);

            return (i == Rank);
        }
    }

    [DebuggerDisplay("V:{Value.ToString()}")]
    public class FibNode<T>
    {
        internal FibNode<T> Parent;
        internal FibNode<T> PreviousSibling;
        internal FibNode<T> NextSibling;

        internal CyclicForrest<T> Children;

        internal bool Marked;

        public T Value;
        public int Weight;

        public FibNode(T value, int weight)
        {
            Value = value;
            Weight = weight;

            ResetSiblingsAndParentToATreeState();
        }

        /// <summary>
        /// Resets node's references to a standalone tree state.
        /// </summary>
        internal void ResetSiblingsAndParentToATreeState()
        {
            Parent = null;

            NextSibling = this;
            PreviousSibling = this;
        }
    }

}
