using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSQ003.ConsistentHash.Library
{
    /// <summary>
    /// Consistant Hash of <c>T</c>
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    public class ConsistentHash<T> where T : class, new()
    {
        /// <summary>
        /// Sorted Dictionary Representing Circle for Consistant Hash
        /// </summary>
        private readonly SortedDictionary<int, T> circle = new SortedDictionary<int, T>();

        /// <summary>
        /// default _replicate count
        /// </summary>
        int _replicate = 100;

        /// <summary>
        /// cache the ordered keys for better performance
        /// </summary>
        int[] ayKeys;

        public ConsistentHash()
        {
            ayKeys = new int[_replicate];
        }

        /// <summary>
        /// Init
        /// <para>
        /// it's better you override the GetHashCode() of T.
        /// </para>
        /// <para>
        /// we will use GetHashCode() to identify different node.
        /// </para>
        /// </summary>
        /// <param name="nodes">List of T</param>
        public void Init(IEnumerable<T> nodes)
        {
            Init(nodes, _replicate);
        }

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="nodes">List of T</param>
        /// <param name="replicate"></param>
        public void Init(IEnumerable<T> nodes, int replicate)
        {
            _replicate = replicate;

            foreach (T node in nodes)
            {
                this.Add(node, false);
            }
            ayKeys = circle.Keys.ToArray();
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="node">Node</param>
        public void Add(T node)
        {
            Add(node, true);
        }

        /// <summary>
        /// Add node of type <c>T</c>
        /// </summary>
        /// <param name="node"></param>
        /// <param name="updateKeyArray">true by default</param>
        private void Add(T node, bool updateKeyArray)
        {
            for (int i = 0; i < _replicate; i++)
            {
                int hash = MurmurHash2.BetterHash(node.GetHashCode().ToString() + i);
                circle[hash] = node;
            }

            if (updateKeyArray)
            {
                ayKeys = circle.Keys.ToArray();
            }
        }

        /// <summary>
        /// Remove a node
        /// </summary>
        /// <param name="node"><c>Ts</c></param>
        public void Remove(T node)
        {
            for (int i = 0; i < _replicate; i++)
            {
                int hash = MurmurHash2.BetterHash(node.GetHashCode().ToString() + i);
                if (!circle.Remove(hash))
                {
                    //throw new Exception("can not remove a node that not added");
                }
            }
            ayKeys = circle.Keys.ToArray();
        }

        /// <summary>
        /// GetNode_slow
        /// <para>
        /// we keep this function just for performance compare
        /// </para>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetNode_slow(String key)
        {
            int hash = MurmurHash2.BetterHash(key);
            if (circle.ContainsKey(hash))
            {
                return circle[hash];
            }

            int first = circle.Keys.FirstOrDefault(h => h >= hash);
            if (first == new int())
            {
                first = ayKeys[0];
            }
            T node = circle[first];
            return node;
        }

        /// <summary>
        /// return the index of first item that >= val, if not exist, return 0.
        /// </summary>
        /// <param name="ay">ay should be ordered array</param>
        /// <param name="val"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Stuff that should never happen</exception>
        int First_ge(int[] ay, int val)
        {
            int begin = 0;
            int end = ay.Length - 1;

            if (ay[end] < val || ay[0] > val)
            {
                return 0;
            }

#pragma warning disable IDE0059 // Unnecessary assignment of a value
            int mid = begin;
#pragma warning restore IDE0059 
            while (end - begin > 1)
            {
                mid = (end + begin) / 2;
                if (ay[mid] >= val)
                {
                    end = mid;
                }
                else
                {
                    begin = mid;
                }
            }

            if (ay[begin] > val || ay[end] < val)
            {
                throw new InvalidOperationException("should not happen");
            }

            return end;
        }

        /// <summary>
        /// Get Key
        /// </summary>
        /// <param name="key">(key)</param>
        /// <returns>Type of <c>T</c></returns>
        public T GetNode(String key)
        {
            //return GetNode_slow(key);

            int hash = MurmurHash2.BetterHash(key);

            int first = First_ge(ayKeys, hash);

            //int diff = circle.Keys[first] - hash;

            return circle[ayKeys[first]];
        }
    }
}
