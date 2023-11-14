using System;
using System.Runtime.InteropServices;
using System.Text;

namespace WSQ003.ConsistentHash.Library
{
    /// <summary>
    /// <![CDATA[https://en.wikipedia.org/wiki/MurmurHash]]>
    /// <para>This is a non-cryptographic hash designed to evenly distribute</para>
    /// </summary>
    public static class MurmurHash2
    {
        private const UInt32 defaultSeed = 0xc58f1a7b;
        private const UInt32 m = 0x5bd1e995;
        private const Int32 r = 24;

        /// <summary>
        /// CTOR w. <c>defaultSeed</c>
        /// </summary>
        /// <param name="data">(data)</param>
        /// <returns>hash</returns>
        public static UInt32 Hash(byte[] data)
        {
            return Hash(data, defaultSeed);
        }

        /// <summary>
        /// Murmur 2 Hash with DIY Seed
        /// </summary>
        /// <param name="data">(data)</param>
        /// <param name="seed">seed</param>
        /// <returns>Murmer 2 Hash</returns>
        public static UInt32 Hash(byte[] data, UInt32 seed)
        {
            Int32 length = data.Length;
            if (length == 0)
                return 0;
            UInt32 h = seed ^ (UInt32)length;
            Int32 currentIndex = 0;

            // array will be length of Bytes but contains Uints
            // therefore the currentIndex will jump with +1
            // while length will jump with +4

            UInt32[] hackArray = new BytetoUInt32Converter { Bytes = data }.UInts;
            while (length >= 4)
            {
                UInt32 k = hackArray[currentIndex++];
                k *= m;
                k ^= k >> r;
                k *= m;

                h *= m;
                h ^= k;
                length -= 4;
            }
            currentIndex *= 4; // fix the length
            switch (length)
            {
                case 3:
                    h ^= (UInt16)(data[currentIndex++] | data[currentIndex++] << 8);
                    h ^= (UInt32)data[currentIndex] << 16;
                    h *= m;
                    break;
                case 2:
                    h ^= (UInt16)(data[currentIndex++] | data[currentIndex] << 8);
                    h *= m;
                    break;
                case 1:
                    h ^= data[currentIndex];
                    h *= m;
                    break;
                default:
                    break;
            }

            // Do a few final mixes of the hash to ensure the last few
            // bytes are well-incorporated.
            h ^= h >> 13;
            h *= m;
            h ^= h >> 15;

            return h;
        }

        /// <summary>
        /// BetterHash that usual XOR
        /// <para>
        /// The default String.GetHashCode() can't well spread strings like "1", "2", "3"
        /// </para>
        /// <para>
        /// Uses <c>MurmurHash2</c>
        /// </para>
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Better Hash</returns>
        public static int BetterHash(string key)
        {
            uint hash = MurmurHash2.Hash(Encoding.ASCII.GetBytes(key));
            return (int)hash;
        }

    }
}
