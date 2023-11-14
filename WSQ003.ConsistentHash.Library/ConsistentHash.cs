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
        #region Fields, etc."
        /// <summary>
        /// Sorted Dictionary Representing Circle for Consistant Hash
        /// </summary>
        private readonly SortedDictionary<int, T> circle = new SortedDictionary<int, T>();

        /// <summary>
        /// default: replicate count
        /// </summary>
        public const int DefaultReplicateCount = 100;

        /// <summary>
        /// Replacate Count
        /// </summary>
        public int ReplacateCount { get; set; }

        /// <summary>
        /// cache the ordered keys for better performance
        /// </summary>
        int[] ayKeys;
        #endregion

        #region "CTOR"

        /// <summary>
        /// CTOR
        /// <para>Follow by a call to <c>Init</c></para>
        /// </summary>
        public ConsistentHash()
        {
            this.ReplacateCount = DefaultReplicateCount;
            ayKeys = new int[this.ReplacateCount];
        }

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="nodes">List of Thing to CH</param>
        public ConsistentHash(IEnumerable<T> nodes): this()
        {
            this.ReplacateCount = DefaultReplicateCount;
            Init(nodes, this.ReplacateCount);
        }

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="nodes">List of Thing to CH</param>
        /// <param name="replacateCount">Replicates</param>
        public ConsistentHash(IEnumerable<T> nodes, int replacateCount)
        {
            this.ReplacateCount = replacateCount;
            Init(nodes, replacateCount);
        }

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="nodes">List of T</param>
        /// <param name="replicateCount">Replacate Count</param>
        private void Init(IEnumerable<T> nodes, int replicateCount)
        {
            this.ReplacateCount = replicateCount;

            foreach (T node in nodes)
            {
                this.AddNode(node, false);
            }
            ayKeys = circle.Keys.ToArray();
        }

        #endregion

        #region "Methods"

        /// <summary>
        /// AddNode
        /// </summary>
        /// <param name="node">Node</param>
        public void AddNode(T node)
        {
            AddNode(node, true);
        }

        /// <summary>
        /// AddNode node of type <c>T</c>
        /// </summary>
        /// <param name="node"></param>
        /// <param name="updateKeyArray">true by default</param>
        private void AddNode(T node, bool updateKeyArray)
        {
            for (int i = 0; i < this.ReplacateCount; i++)
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
        /// Remove Node 
        /// </summary>
        /// <param name="node">of <c>T</c></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void RemoveNode(T node)
        {
            for (int i = 0; i < this.ReplacateCount; i++)
            {
                int hash = MurmurHash2.BetterHash(node.GetHashCode().ToString() + i);
                if (!circle.Remove(hash))
                {
                    throw new InvalidOperationException("can not remove a node that not added");
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
        private T GetNode_slow(String key)
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
        /// <param name="val">find first mapped node</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Stuff that should never happen</exception>
        private static int First_ge(int[] ay, int val)
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

        #endregion

        #region "Map a key onto a node"

        /// <summary>
        /// Get Key
        /// </summary>
        /// <param name="key">(key)</param>
        /// <returns>Type of <c>T</c></returns>
        public T MapKeyToNode(string key)
        {
            //return GetNode_slow(key);

            int hash = MurmurHash2.BetterHash(key);

            int first = First_ge(ayKeys, hash);

            //int diff = circle.Keys[first] - hash;

            return circle[ayKeys[first]];
        }

        #endregion
    }

}
