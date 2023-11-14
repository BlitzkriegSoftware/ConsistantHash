using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using WSQ003.ConsistentHash.Library.Tests.Models;

namespace WSQ003.ConsistentHash.Library.Tests
{
    /// <summary>
    /// For of test from WSQ003
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class Wsq003ConHashTests
    {
        #region "Test Boilerplate"
        private static TestContext _testContext;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            _testContext = context;
        }
        #endregion

        /// <summary>
        /// See <![CDATA[https://github.com/wsq003/consistent-hash/blob/master/test.cs]]>
        /// </summary>
        [TestMethod]
        public void Test_From_WSQ003_01()
        {
            int search = 100000;
            DateTime start = DateTime.UtcNow;

            List<Server> servers = new();
            for (int i = 0; i < 1000; i++)
            {
                servers.Add(new Server(i));
            }

            ConsistentHash<Server> ch = new(servers);

            SortedList<int, int> ay1 = new();
            for (int i = 0; i < search; i++)
            {
                int temp = ch.MapKeyToNode(i.ToString()).Id;
                ay1[i] = temp;
            }

            TimeSpan ts = DateTime.UtcNow - start;
            _testContext.WriteLine(search + " each use macro seconds: " + (ts.TotalMilliseconds / search) * 1000);

            ch.RemoveNode(servers[1]);
            SortedList<int, int> ay2 = new();
            for (int i = 0; i < search; i++)
            {
                int temp = ch.MapKeyToNode(i.ToString()).Id;

                ay2[i] = temp;
            }

            int diff = 0;
            for (int i = 0; i < search; i++)
            {
                if (ay1[i] != ay2[i])
                {
                    diff++;
                }
            }

            _testContext.WriteLine("diff: " + diff);
        }
    }
}