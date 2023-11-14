using System;
using System.Runtime.InteropServices;
using System.Text;

namespace WSQ003.ConsistentHash.Library
{
    /// <summary>
    /// <![CDATA[https://github.com/wsq003/consistent-hash]]>
    /// </summary>
    public class MurmurHash2
    {
        const UInt32 defaultSeed = 0xc58f1a7b;
        const UInt32 m = 0x5bd1e995;
        const Int32 r = 24;

        /// <summary>
        /// CTOR w. <c>defaultSeed</c>
        /// </summary>
        /// <param name="data">(data)</param>
        /// <returns>hash</returns>
        public static UInt32 Hash(Byte[] data)
        {
            return Hash(data, defaultSeed);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">(data)</param>
        /// <param name="seed">seed</param>
        /// <returns>hash</returns>
        public static UInt32 Hash(Byte[] data, UInt32 seed)
        {
            Int32 length = data.Length;
            if (length == 0)
                return 0;
            UInt32 h = seed ^ (UInt32)length;
            Int32 currentIndex = 0;

            // array will be length of Bytes but contains Uints
            // therefore the currentIndex will jump with +1 while length will jump with +4

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
        /// BetterHash
        /// <para>
        /// default String.GetHashCode() can't well spread strings like "1", "2", "3"
        /// </para>
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Better Hash</returns>
        public static int BetterHash(String key)
        {
            uint hash = MurmurHash2.Hash(Encoding.ASCII.GetBytes(key));
            return (int)hash;
        }

    }
}
