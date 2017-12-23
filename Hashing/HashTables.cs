using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Hashing
{

    public interface IHashTable<TValue, TKey>
    {
        void Add(TKey key, TValue value);
        (bool found, TValue value) Find(TKey key);

    }
    public abstract class HashTable<TValue, TKey>
    {
        protected EqualityComparer<TValue> comparer;
        protected KeyValuePair<TKey, TValue>[] dataStore;

        public HashTable(int logOfCapacity)
        {
            dataStore = new KeyValuePair<TKey, TValue>[1 << logOfCapacity];
            comparer = EqualityComparer<TValue>.Default;
        }
    }

    public sealed class LinearHashTable<T> : HashTable<T, (bool Occupied, ulong Key)>, IHashTable<T, ulong> where T : IEquatable<T>
    {
        IHashFunction hashFunction;

        public LinearHashTable(int logOfCapacity, IHashFunction hash)
            : base(logOfCapacity)
        {
            hashFunction = hash;
        }

        public void Add(ulong key, T value)
        {
            int index = hashFunction.Hash(key);
            while(!IsEmpty(index))
            {
                index++;
                if (index >= dataStore.Length) { index = 0; }
            }

            dataStore[index] = new KeyValuePair<(bool, ulong), T>((true, key), value);
        }

        public (bool found, T value) Find(ulong key)
        {
            int index = hashFunction.Hash(key);

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
    }

    public enum HashedWith { Empty = 0, FunctionA, FunctionB }
    public sealed class CuckooHashTable<T> : HashTable<T, (ulong key, HashedWith type)>, IHashTable<T, ulong>
    {
        IHashFunction functionA;
        IHashFunction functionB;

        private int numberOfElements = 0;
        private int rebuildTreshold = 0;

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

            if (!TryToAdd(key, value))
            {
                Rebuild(key, value);
            }
        }

        private bool TryToAdd(ulong key, T value)
        {
            int hashA = functionA.Hash(key);
            if (IsEmpty(hashA)) { dataStore[hashA] = new KeyValuePair<(ulong key, HashedWith type), T>((key, HashedWith.FunctionA), value); return true; }

            int hashB = functionB.Hash(key);
            return TryToAddAndPotentiallyMoveOriginal(key, value, hashB);
        }

        private bool TryToAddAndPotentiallyMoveOriginal(ulong key, T value, int index)
        {
            int movedElements = 0;

            var elementToBeMoved = dataStore[index];
            dataStore[index] = new KeyValuePair<(ulong key, HashedWith type), T>((key, HashedWith.FunctionB), value);

            while (elementToBeMoved.Key.type != HashedWith.Empty)
            {
                movedElements++;
                if (movedElements > rebuildTreshold)
                {
                    // The element we tried to add originally has to be on one of the following indexes
                    // .. find out where and replace with with the element currently being moved to 
                    // .. put the dataStore into pre-Add state for the rebuild process. 
                    var hashA = functionA.Hash(key);
                    if (dataStore[hashA].Key.key == key) { dataStore[hashA] = elementToBeMoved; return false; }


                    var hashB = functionB.Hash(key);
                    Debug.Assert(dataStore[hashB].Key.key == key);
                    dataStore[hashB] = elementToBeMoved;
                    return false;
                }

                var newFunction = HashedWith.Empty;
                var newIndex = 0;

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

                var newElementToBeMoved = dataStore[newIndex];
                dataStore[newIndex] = new KeyValuePair<(ulong key, HashedWith type), T>((elementToBeMoved.Key.key, newFunction), elementToBeMoved.Value);
                elementToBeMoved = newElementToBeMoved;
            }

            return true;

        }

        private void Rebuild(ulong key, T value)
        {
            var originalDataStore = dataStore;
            dataStore = new KeyValuePair<(ulong key, HashedWith type), T>[originalDataStore.Length];

            while (true)
            {
                functionA.Reset();
                functionB.Reset();

                bool rebuildFailed = false;

                // add the element that caused the rebuild first (it's guaranteed to succeed)
                TryToAdd(key, value);

                // add all original elements from the dataStore
                for (int i = 0; i < originalDataStore.Length; i++)
                {
                    if (originalDataStore[i].Key.type == HashedWith.Empty) { continue; }
                    if (!TryToAdd(originalDataStore[i].Key.key, originalDataStore[i].Value))
                    {
                        CleanDataStore();
                        rebuildFailed = true;
                        break;
                    }
                }

                // when all are added, break the infinate loop and return from this function
                if (!rebuildFailed)
                { return; }
            }
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
    }
}
