using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Hashing
{
    public class LinearHashTable<T> where T : IEquatable<T>
    {
        IHashFunction hashFunction;
        KeyValuePair<ulong, T>[] dataStore;

        EqualityComparer<T> comparer;


        public LinearHashTable(int logOfCapacity, IHashFunction hash)
        {
            hashFunction = hash;
            dataStore = new KeyValuePair<ulong, T>[1 << logOfCapacity];

            comparer = EqualityComparer<T>.Default;
        }

        public void Add(ulong key, T value)
        {
            // Do not allow inserting default(T) values
            Debug.Assert(!comparer.Equals(value, default(T)));

            ulong index = hashFunction.Hash(key);
            while(!comparer.Equals(dataStore[index].Value, default(T)))
            {
                index++;
                if (index >= (ulong)dataStore.Length) { index = 0; }
            }

            dataStore[index] = new KeyValuePair<ulong, T>(key, value);
        }

        public T Find(ulong key)
        {
            ulong index = hashFunction.Hash(key);

            while (dataStore[index].Key != key)
            {
                if(comparer.Equals(dataStore[index].Value, default(T))) { return default(T); }
                index++;
                if (index >= (ulong)dataStore.Length) { index = 0; }
            }

            return dataStore[index].Value;
        }
    }
}
