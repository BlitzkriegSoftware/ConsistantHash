using System.Diagnostics.CodeAnalysis;

namespace WSQ003.ConsistentHash.Library.Tests.Models
{
    /// <summary>
    /// Test Class
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Server
    {
        #region "CTOR"
        public Server()
        {
            this.Id = 0;
        }

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="id"></param>
        public Server(int id)
        {
            Id = id;
        }

        #endregion

        #region "Properties"

        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Count of mapped keys
        /// </summary>
        public double Count { get; set; }

        #endregion

        #region "Overrides"

        /// <summary>
        /// Get Hash Code
        /// <para>Uses <c>BetterHash</c></para>
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ConsistentHash.Library.MurmurHash2.BetterHash(this.Id.ToString());
        }

        /// <summary>
        /// To String
        /// </summary>
        /// <returns>Server Name</returns>
        public override string ToString()
        {
            return $"Id: {this.Id}, Count: {this.Count}";
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj">Thing to test</param>
        /// <returns>Equals</returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj is not Server x) return false;
            return x.Id == Id;
        }

        #endregion
    }
}