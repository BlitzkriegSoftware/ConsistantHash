using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using WSQ003.ConsistentHash.Library.Tests.Libs;
using WSQ003.ConsistentHash.Library.Tests.Models;

namespace WSQ003.ConsistentHash.Library.Tests
{
    /// <summary>
    /// More tests to explore use
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class BlitzTests
    {
        #region "Test Boilerplate"
        private static TestContext _testContext;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            _testContext = context;
        }
        #endregion

        [TestMethod]
        public void Map_Keys_Onto_Servers()
        {
            // --- Arrange
            #region "Make a bunch of keys"
            int keyCount = 10000;
            var keys = new List<KeyItem>(keyCount);
            for (int i = 0; i < keyCount; i++)
            {
                var key = Libs.KeyMaker.Keyer();
                keys.Add(new KeyItem()
                {
                    Key = key,
                    Slot =0
                });
            }
            #endregion

            #region Create "Server Farm"
            int serverCount = 10;
            var servers = new List<Models.Server>();
            for (int i = 0; i < serverCount; i++)
            {
                servers.Add(new Models.Server { Id = i });
            }
            #endregion

            // --- Act
            ConsistentHash<Server> ch = new(servers);
            foreach (var key in keys)
            {
                var svr = ch.MapKeyToNode(key.Key);
                key.Slot = svr.Id;
                svr.Count++;
                // _testContext.WriteLine($"{key} mapped to server {svr.Id}");
            }

            // --- Assert

            // Do some reporting
            _testContext.WriteLine("\nDistribution of Keys By Server");
            foreach (var svr in servers)
            {
                _testContext.WriteLine(svr.ToString());
            }

            var counts = servers.AsQueryable().Select(s => s.Count).ToList();
            double avg = counts.Average();
            double stdDev = counts.StandardDeviation();
            _testContext.WriteLine($"\nServer Distro::\n\tCount: {keyCount}, Average: {avg:n2}, StdDev: {stdDev:n3}");

            // Test that keys from our list maps to where we got before
            int testSamples = 10;
            for(int i = 0;i < testSamples; i++)
            {
                var index = Libs.KeyMaker.Dice.Next(keys.Count());
                var ki = keys[index];
                var expected = ki.Slot;
                var actual = ch.MapKeyToNode(ki.Key).Id;
                Assert.AreEqual(expected, actual);
            }
        }

    }
}
