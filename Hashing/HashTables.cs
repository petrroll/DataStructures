#define LOGGER
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Hashing
{

    public interface IHashTable<TValue, TKey>
    {
#if LOGGER
        StepsCounter Logger { get; set; }
#endif
        void Add(TKey key, TValue value);
        (bool found, TValue value) Find(TKey key);

        void Reset();
    }

    public abstract class HashTable<TValue, TKey>
    {
        protected KeyValuePair<TKey, TValue>[] dataStore;

        public HashTable(int logOfCapacity)
        {
            dataStore = new KeyValuePair<TKey, TValue>[1 << logOfCapacity];
        }

        public virtual void Reset()
        {
            for (int i = 0; i < dataStore.Length; i++)
            {
                dataStore[i] = new KeyValuePair<TKey, TValue>();
            }
        }
    }

    public sealed class LinearHashTable<T> : HashTable<T, (bool Occupied, ulong Key)>, IHashTable<T, ulong> where T : IEquatable<T>
    {
        IHashFunction hashFunction;

#if LOGGER
        public StepsCounter Logger { get; set; }
#endif

        public LinearHashTable(int logOfCapacity, IHashFunction hash)
            : base(logOfCapacity)
        {
            hashFunction = hash;
        }

        public void Add(ulong key, T value)
        {

            int index = hashFunction.Hash(key);

            // find first empty index
            while(!IsEmpty(index))
            {
#if LOGGER
                // add one to logger for each non-empty index we must access
                Logger?.AddStep();
#endif
                index++;
                if (index >= dataStore.Length) { index = 0; } 
            }

            dataStore[index] = new KeyValuePair<(bool, ulong), T>((true, key), value);
        }

        public (bool found, T value) Find(ulong key)
        {
            int index = hashFunction.Hash(key);

            // find the element whose key is the same as the one we're trying to retrieve
            while (dataStore[index].Key.Key != key)
            {
                if (IsEmpty(index)) { return (false, default(T)); }
                index++;
                if (index >= dataStore.Length) { index = 0; }
            }

            return (true, dataStore[index].Value);
        }

        bool IsEmpty(int index)
        {
            return !dataStore[index].Key.Occupied;
        }

        public override void Reset()
        {
            base.Reset();
            hashFunction.Reset();
        }
    }

    public enum HashedWith { Empty = 0, FunctionA, FunctionB }
    public sealed class CuckooHashTable<T> : HashTable<T, (ulong key, HashedWith type)>, IHashTable<T, ulong>
    {
#if LOGGER
        public StepsCounter Logger { get; set; }
#endif

        IHashFunction functionA;
        IHashFunction functionB;

        int numberOfElements = 0;
        int rebuildTreshold = 0;

        public CuckooHashTable(int logOfCapacity, IHashFunction functionA, IHashFunction functionB)
            :base(logOfCapacity)
        {
            this.functionA = functionA;
            this.functionB = functionB;
        }

        public void Add(ulong key, T value)
        {
            numberOfElements++;
            rebuildTreshold = (int)Math.Ceiling(6 * Math.Log(numberOfElements, 2));

            // try to add an element if it fails rebuild the whole datastructure
            if (!TryToAdd(key, value)) { Rebuild(key, value); }
        }

        private bool TryToAdd(ulong key, T value)
        {
            // try to add to an index computed by functionA, potentially move the element that was there originally
            int hashA = functionA.Hash(key);
            return TryToAddAndPotentiallyMoveOriginal(key, value, hashA);
        }

        private bool TryToAddAndPotentiallyMoveOriginal(ulong key, T value, int index)
        {
            int movedElements = 0;

            var elementToBeMoved = dataStore[index];
            dataStore[index] = new KeyValuePair<(ulong key, HashedWith type), T>((key, HashedWith.FunctionA), value);

            // if the element to be moved is empty we can stop working and return true
            while (elementToBeMoved.Key.type != HashedWith.Empty)
            {
#if LOGGER
                // we've swapped some other element with the current elementToBeMoved
                Logger?.AddStep();
#endif

                movedElements++;
                if (movedElements > rebuildTreshold)
                {
                    // If the element we tried to add is the one currentrly being moved then the 
                    // .. datastore is already ready for rebuild. 
                    if(key == elementToBeMoved.Key.key) { return false; }

                    // The element we tried to add originally has to be on one of the following indexes
                    // .. find out where and replace it with with the element currently being moved to 
                    // .. put the dataStore into a pre-Add state for the rebuild process (that gets the 
                    // .. element that was being added separately.
                    var hashA = functionA.Hash(key);
                    if (dataStore[hashA].Key.key == key) { dataStore[hashA] = elementToBeMoved; return false; }

                    var hashB = functionB.Hash(key);
                    Debug.Assert(dataStore[hashB].Key.key == key);
                    dataStore[hashB] = elementToBeMoved;
                    return false;
                }

                // compute new index for the element that is being moved
                var newFunction = HashedWith.Empty;
                var newIndex = 0;

                Debug.Assert(elementToBeMoved.Key.type != HashedWith.Empty);
                if (elementToBeMoved.Key.type == HashedWith.FunctionA)
                {
                    newFunction = HashedWith.FunctionB;
                    newIndex = functionB.Hash(elementToBeMoved.Key.key);
                }
                else
                {
                    newFunction = HashedWith.FunctionA;
                    newIndex = functionA.Hash(elementToBeMoved.Key.key);
                }

                // store the element that is being moved into its new index, potentially move the original inhabitant of that place
                var newElementToBeMoved = dataStore[newIndex];
                dataStore[newIndex] = new KeyValuePair<(ulong key, HashedWith type), T>((elementToBeMoved.Key.key, newFunction), elementToBeMoved.Value);
                elementToBeMoved = newElementToBeMoved;
            }

            return true;

        }

        private void Rebuild(ulong key, T value)
        {
            // save the original array & create a new one into which all values will get copied with new hashing functions
            var originalDataStore = dataStore;
            dataStore = new KeyValuePair<(ulong key, HashedWith type), T>[originalDataStore.Length];

            bool failed;
            do
            {
                functionA.Reset();
                functionB.Reset();

                failed = false;

                // add the element that caused the rebuild (it's guaranteed to succeed because it's the first element)
                TryToAdd(key, value);

                // add all the original elements from the dataStore
                for (int i = 0; i < originalDataStore.Length; i++)
                {
                    var currElement = originalDataStore[i];
                    if (currElement.Key.type == HashedWith.Empty) { continue; } // No need to work on empty cells from the backing array.

                    // If adding an element fails clean the dataStore and repeat the process.
                    if (!TryToAdd(originalDataStore[i].Key.key, originalDataStore[i].Value))
                    {
                        CleanDataStore();
                        failed = true;
                        break;
                    }
                }

            // if failed to move all elements repeat the process with new functions
            } while (failed);
            
        }

        private void CleanDataStore()
        {
            for (int j = 0; j < dataStore.Length; j++)
            {
                dataStore[j] = new KeyValuePair<(ulong key, HashedWith type), T>();
            }
        }

        private bool IsEmpty(int index)
        {
            var dataStorePackage = dataStore[index];
            return (dataStorePackage.Key.type == HashedWith.Empty);
        }

        public (bool found, T value) Find(ulong key)
        {
            var hashA = functionA.Hash(key);
            if(dataStore[hashA].Key.key == key && dataStore[hashA].Key.type != HashedWith.Empty) { return (true, dataStore[hashA].Value); }

            var hashB = functionB.Hash(key);
            if (dataStore[hashB].Key.key == key && dataStore[hashB].Key.type != HashedWith.Empty) { return (true, dataStore[hashB].Value); }

            return (false, default(T));
        }

        public override void Reset()
        {
            base.Reset();

            functionA.Reset();
            functionB.Reset();

            numberOfElements = 0;
        }
    }
}
