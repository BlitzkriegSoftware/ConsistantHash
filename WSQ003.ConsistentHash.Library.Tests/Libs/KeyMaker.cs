using BlitzkriegSoftware.SecureRandomLibrary;
using System.Text;

namespace WSQ003.ConsistentHash.Library.Tests.Libs
{
    /// <summary>
    /// Key Maker
    /// </summary>
    public static class KeyMaker
    {
        /// <summary>
        /// Secure Random
        /// </summary>
        public static readonly SecureRandom Dice = new();

        public const int DefaultKeyLength = 32;
        public const string allowedChars = "abc0123de!fghj4567kmnp#qrstuv$wxyz89-.";

        public static string Keyer(int length = DefaultKeyLength)
        {
            StringBuilder sb = new();
            for(int i = 0; i < length; i++)
            {
                var index = Dice.Next(allowedChars.Length -1);
                sb.Append(allowedChars[index]);
            }
            return sb.ToString();
        }

    }
}
