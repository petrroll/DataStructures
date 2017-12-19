using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Hashing
{
    public abstract class HashTable<T>
    {
        protected EqualityComparer<T> comparer;
        protected KeyValuePair<ulong, T>[] dataStore;

        public HashTable(int logOfCapacity)
        {
            dataStore = new KeyValuePair<ulong, T>[1 << logOfCapacity];
            comparer = EqualityComparer<T>.Default;
        }

        protected bool IsEmpty(int index)
        {
            return comparer.Equals(dataStore[index].Value, default(T));
        }
    }

    public sealed class LinearHashTable<T> : HashTable<T> where T : IEquatable<T>
    {
        IHashFunction hashFunction;

        public LinearHashTable(int logOfCapacity, IHashFunction hash)
            : base(logOfCapacity)
        {
            hashFunction = hash;
        }

        public void Add(ulong key, T value)
        {
            // Do not allow inserting default(T) values
            Debug.Assert(!comparer.Equals(value, default(T)));

            int index = hashFunction.Hash(key);
            while(!IsEmpty(index))
            {
                index++;
                if (index >= dataStore.Length) { index = 0; }
            }

            dataStore[index] = new KeyValuePair<ulong, T>(key, value);
        }

        public T Find(ulong key)
        {
            int index = hashFunction.Hash(key);

            while (dataStore[index].Key != key)
            {
                if (IsEmpty(index)) { return default(T); }
                index++;
                if (index >= dataStore.Length) { index = 0; }
            }

            return dataStore[index].Value;
        }
    }

    public sealed class CuckooHashTable<T> : HashTable<T>
    {
        IHashFunction functionA;
        IHashFunction functionB;

        public CuckooHashTable(int logOfCapacity, IHashFunction functionA, IHashFunction functionB)
            :base(logOfCapacity)
        {
            this.functionA = functionA;
            this.functionB = functionB;
        }

        public void Add(ulong key, T value)
        {
            // Do not allow inserting default(T) values
            Debug.Assert(!comparer.Equals(value, default(T)));

            int hashA = functionA.Hash(key);
            if (IsEmpty(hashA)) { dataStore[hashA] = new KeyValuePair<ulong, T>(key, value); return; }

            int hashB = functionB.Hash(key);
        }

        public T Find(ulong key)
        {
            return default(T);
        }
    }
}
