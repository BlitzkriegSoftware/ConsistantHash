using System;
using System.Runtime.InteropServices;

namespace WSQ003.ConsistentHash.Library
{
    /// <summary>
    /// BytetoUInt32Converter
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct BytetoUInt32Converter
    {
        /// <summary>
        /// Bytes
        /// </summary>
        [FieldOffset(0)]
        public Byte[] Bytes;

        /// <summary>
        /// UInts
        /// </summary>
        [FieldOffset(0)]
        public UInt32[] UInts;
    }
}
