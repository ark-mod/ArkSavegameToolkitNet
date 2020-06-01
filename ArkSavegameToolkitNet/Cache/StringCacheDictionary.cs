using System;

namespace ArkSavegameToolkitNet.Cache
{
    static class HashHelpers
    {
        internal const Int32 HashPrime = 101;

        public static readonly int[] primes = {
            3, 7, 11, 17, 23, 29, 37, 47, 59, 71, 89, 107, 131, 163, 197, 239, 293, 353, 431, 521, 631, 761, 919,
            1103, 1327, 1597, 1931, 2333, 2801, 3371, 4049, 4861, 5839, 7013, 8419, 10103, 12143, 14591,
            17519, 21023, 25229, 30293, 36353, 43627, 52361, 62851, 75431, 90523, 108631, 130363, 156437,
            187751, 225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403, 968897, 1162687, 1395263,
            1674319, 2009191, 2411033, 2893249, 3471899, 4166287, 4999559, 5999471, 7199369};

        public static bool IsPrime(int candidate)
        {
            if ((candidate & 1) != 0)
            {
                int limit = (int)Math.Sqrt(candidate);
                for (int divisor = 3; divisor <= limit; divisor += 2)
                {
                    if ((candidate % divisor) == 0)
                        return false;
                }
                return true;
            }
            return (candidate == 2);
        }

        public static int GetPrime(int min)
        {
            if (min < 0)
                throw new ArgumentException();

            for (int i = 0; i < primes.Length; i++)
            {
                int prime = primes[i];
                if (prime >= min) return prime;
            }

            //outside of our predefined table. 
            //compute the hard way. 
            for (int i = (min | 1); i < Int32.MaxValue; i += 2)
            {
                if (IsPrime(i) && ((i - 1) % HashPrime != 0))
                    return i;
            }
            return min;
        }

        public static int ExpandPrime(int oldSize)
        {
            int newSize = 2 * oldSize;

            //// Allow the hashtables to grow to maximum possible size (~2G elements) before encoutering capacity overflow.
            //// Note that this check works even when _items.Length overflowed thanks to the (uint) cast
            //if ((uint)newSize > MaxPrimeArrayLength && MaxPrimeArrayLength > oldSize)
            //{
            //    Contract.Assert(MaxPrimeArrayLength == GetPrime(MaxPrimeArrayLength), "Invalid MaxPrimeArrayLength");
            //    return MaxPrimeArrayLength;
            //}

            return GetPrime(newSize);
        }
    }

    class StringCacheDictionary<TValue>
    {
        private struct Entry
        {
            public int hashCode;    // Lower 31 bits of hash code, -1 if unused
            public int next;        // Index of next entry, -1 if last
            public string key;           // Key of entry
            public TValue value;         // Value of entry
        }

        private int[] buckets;
        private Entry[] entries;
        private int count;
        private int version;
        private int freeList;
        private int freeCount;
        //private IEqualityComparer<TKey> comparer;
        //private KeyCollection keys;
        //private ValueCollection values;
        //private Object _syncRoot;

        private void Initialize(int capacity)
        {
            int size = HashHelpers.GetPrime(capacity);
            buckets = new int[size];
            for (int i = 0; i < buckets.Length; i++) buckets[i] = -1;
            entries = new Entry[size];
            freeList = -1;
        }

        public bool TryGetValue(ReadOnlySpan<char> key, out TValue value)
        {
            int i = FindEntry(key);
            if (i >= 0)
            {
                value = entries[i].value;
                return true;
            }
            value = default(TValue);
            return false;
        }

        private int FindEntry(ReadOnlySpan<char> key)
        {
            if (key == null)
            {
                throw new ArgumentNullException();
            }

            if (buckets != null)
            {
                //int hashCode = comparer.GetHashCode(key) & 0x7FFFFFFF;
                int hashCode = string.GetHashCode(key) & 0x7FFFFFFF;
                for (int i = buckets[hashCode % buckets.Length]; i >= 0; i = entries[i].next)
                {
                    //if (entries[i].hashCode == hashCode && comparer.Equals(entries[i].key, key)) return i;
                    if (entries[i].hashCode == hashCode && MemoryExtensions.SequenceEqual(key, entries[i].key)) return i;
                }
            }
            return -1;
        }

        public bool GetOrAdd(ReadOnlySpan<char> key, Func<string, TValue> create, out TValue value)
        {
            if (key == null)
            {
                throw new ArgumentNullException();
            }

            if (buckets == null) Initialize(0);
            //int hashCode = comparer.GetHashCode(key) & 0x7FFFFFFF;
            int hashCode = string.GetHashCode(key) & 0x7FFFFFFF;
            int targetBucket = hashCode % buckets.Length;


            for (int i = buckets[targetBucket]; i >= 0; i = entries[i].next)
            {
                //if (entries[i].hashCode == hashCode && comparer.Equals(entries[i].key, key))
                if (entries[i].hashCode == hashCode && MemoryExtensions.SequenceEqual(key, entries[i].key))
                {
                    value = entries[i].value;
                    return true;
                }

            }
            int index;
            if (freeCount > 0)
            {
                index = freeList;
                freeList = entries[index].next;
                freeCount--;
            }
            else
            {
                if (count == entries.Length)
                {
                    Resize();
                    targetBucket = hashCode % buckets.Length;
                }
                index = count;
                count++;
            }

            entries[index].hashCode = hashCode;
            entries[index].next = buckets[targetBucket];
            entries[index].key = key.ToString();
            entries[index].value = create != null ? create(key.ToString()) : typeof(TValue) == typeof(string) ? (dynamic)key.ToString() : default(TValue);
            buckets[targetBucket] = index;
            version++;

            value = entries[index].value;

            return false;
        }

        public void Add(string key, TValue value)
        {
            Insert(key, value, true);
        }

        private void Insert(string key, TValue value, bool add)
        {

            if (key == null)
            {
                throw new ArgumentNullException();
            }

            if (buckets == null) Initialize(0);
            //int hashCode = comparer.GetHashCode(key) & 0x7FFFFFFF;
            int hashCode = key.GetHashCode() & 0x7FFFFFFF;
            int targetBucket = hashCode % buckets.Length;

            for (int i = buckets[targetBucket]; i >= 0; i = entries[i].next)
            {
                //if (entries[i].hashCode == hashCode && comparer.Equals(entries[i].key, key))
                if (entries[i].hashCode == hashCode && key.Equals(entries[i].key))
                {
                    if (add)
                    {
                        throw new ArgumentException();
                    }
                    entries[i].value = value;
                    version++;
                    return;
                }
            }
            int index;
            if (freeCount > 0)
            {
                index = freeList;
                freeList = entries[index].next;
                freeCount--;
            }
            else
            {
                if (count == entries.Length)
                {
                    Resize();
                    targetBucket = hashCode % buckets.Length;
                }
                index = count;
                count++;
            }

            entries[index].hashCode = hashCode;
            entries[index].next = buckets[targetBucket];
            entries[index].key = key.ToString();
            entries[index].value = value;
            buckets[targetBucket] = index;
            version++;
        }

        private void Resize()
        {
            Resize(HashHelpers.ExpandPrime(count), false);
        }

        private void Resize(int newSize, bool forceNewHashCodes)
        {
            //Contract.Assert(newSize >= entries.Length);

            int[] newBuckets = new int[newSize];
            for (int i = 0; i < newBuckets.Length; i++) newBuckets[i] = -1;
            Entry[] newEntries = new Entry[newSize];
            Array.Copy(entries, 0, newEntries, 0, count);
            if (forceNewHashCodes)
            {
                for (int i = 0; i < count; i++)
                {
                    if (newEntries[i].hashCode != -1)
                    {
                        //newEntries[i].hashCode = (comparer.GetHashCode(newEntries[i].key) & 0x7FFFFFFF);
                        newEntries[i].hashCode = (newEntries[i].key.GetHashCode() & 0x7FFFFFFF);
                    }
                }
            }
            for (int i = 0; i < count; i++)
            {
                if (newEntries[i].hashCode >= 0)
                {
                    int bucket = newEntries[i].hashCode % newSize;
                    newEntries[i].next = newBuckets[bucket];
                    newBuckets[bucket] = i;
                }
            }
            buckets = newBuckets;
            entries = newEntries;
        }
    }
}
