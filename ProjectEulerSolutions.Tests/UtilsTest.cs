using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectEulerSolutions.Models;
using System.Linq;
using System.Collections.Generic;

namespace ProjectEulerSolutions.Tests
{
    [TestClass]
    public class UtilsTest
    {
        private static IEnumerable<ulong> BuildSeries(ulong start, ulong dist, int numEntries)
        {
            return Enumerable.Range(0, numEntries).Select(i => start + dist * (ulong) i);
        }

        // Check list so that list length >= numEntries and diff between consecutive elements
        // are all dist.
        private static bool CheckSeries(IList<ulong> series, ulong dist, int numEntries)
        {
            if (series.Count() < numEntries)
            {
                return false;
            }

            // Difference between consecutive entry in series must be dist.
            var diff = series.Zip(series.Skip(1), Tuple.Create).Select(t => t.Item2 - t.Item1);
            if (diff.Any(d => d != dist))
            {
                return false;
            }

            return true;
        }

        // Check list so that list length >= numEntries and diff between consecutive elements
        // are all the same.
        private static bool CheckSeries(IList<ulong> series, int numEntries)
        {
            ulong dist = series[1] - series[0];
            return CheckSeries(series, dist, numEntries);
        }

        // Check series of lists
        private static bool CheckSeries(IEnumerable<IList<ulong>> series, ulong dist, int numEntries)
        {
            return series.All(e => CheckSeries(e, dist, numEntries));
        }

        // Check series of lists
        private static bool CheckSeries(IEnumerable<IList<ulong>> series, int numEntries)
        {
            return series.All(e => CheckSeries(e, numEntries));
        }

        [TestMethod]
        public void TestFindArithemeticNormalSeries()
        {
            // 10,15,20,25,30,35
            ulong dist = 5;
            int numEntries = 6;
            var series = BuildSeries(10, dist, numEntries).ToArray();

            // 10,15,20,25,30,35
            var ans1 = Utils.FindArithemeticSeries(series, (uint)numEntries).ToArray();
            Assert.IsTrue(CheckSeries(ans1, dist, numEntries));
            Assert.AreEqual(1, ans1.Count());

            // 10..35 15..35
            var ans2 = Utils.FindArithemeticSeries(series, (uint)(numEntries - 1)).ToArray();
            Assert.IsTrue(CheckSeries(ans2, dist, (numEntries - 1)));
            Assert.AreEqual(2, ans2.Count());

            // 10..35 15..35 20..35
            var ans3 = Utils.FindArithemeticSeries(series, (uint)(numEntries - 2)).ToArray();
            Assert.IsTrue(CheckSeries(ans3, dist, (numEntries - 2)));
            Assert.AreEqual(3, ans3.Count());
        }

        [TestMethod]
        public void TestFindArithemeticDirtySeries()
        {
            // 10,11,15,17,20,25,30,32,35
            ulong dist = 5;
            int numEntries = 6;
            var series = BuildSeries(10, dist, numEntries).ToList();
            series.Add(11);
            series.Add(17);
            series.Add(32);
            series.Sort();

            // 10,15,20,25,30,35
            var ans1 = Utils.FindArithemeticSeries(series, (uint)numEntries).ToArray();
            Assert.IsTrue(CheckSeries(ans1, dist, numEntries));

            // 10..35 15..35
            var ans2 = Utils.FindArithemeticSeries(series, (uint)(numEntries - 1)).ToArray();
            Assert.IsTrue(CheckSeries(ans2, dist, (numEntries - 1)));
            Assert.AreEqual(2, ans2.Count());

            // 10..35 15..35 20..35
            var ans3 = Utils.FindArithemeticSeries(series, (uint)(numEntries - 2)).ToArray();
            Assert.IsTrue(CheckSeries(ans3, dist, (numEntries - 2)));
            Assert.AreEqual(3, ans3.Count());
        }

        [TestMethod]
        public void TestFindArithemeticTwoSeries()
        {
            // 10,15,20,25,30,35
            var series1 = BuildSeries(10, 5, 6);
            // 11,18,25,32,39
            var series2 = BuildSeries(11, 7, 5);
            // 10,11,15,18,20,25,30,32,35,39
            var series = series1.Concat(series2).Distinct().ToList();
            series.Sort();

            // 10..35
            var ans1 = Utils.FindArithemeticSeries(series, 6).ToArray();
            Assert.IsTrue(CheckSeries(ans1, 6));
            Assert.IsTrue(CheckSeries(ans1, 5, 6));

            // 10..35 15..35 11..39
            var ans2 = Utils.FindArithemeticSeries(series, 5).ToArray();
            Assert.IsTrue(CheckSeries(ans2, 5));
            Assert.AreEqual(3, ans2.Count());

            // 10..35 15..35 20..35 11..39 18..39
            var ans3 = Utils.FindArithemeticSeries(series, 4).ToArray();
            Assert.IsTrue(CheckSeries(ans3, 4));
            Assert.AreEqual(5, ans3.Count());
        }

        [TestMethod]
        public void TestFindArithmeticDirtyMultipleSeries()
        {
            var series = new List<ulong>();
            for (int i = 2; i < 6; i++)
            {
                // 2,6,10,14,18,22...
                // 3,12,21,30,39...
                // 4,20,36,...
                // 5,30,55,...
                series.AddRange(BuildSeries((ulong)i, (ulong)(i * i), 10 - i));
            }
            series.Add(7);
            series.Add(11);
            series.Add(23);
            series.Add(31);
            series = series.Distinct().ToList();
            series.Sort();

            for (int i = 2; i < 6; i++)
            {
                uint len = (uint)(10 - i);
                var ans = Utils.FindArithemeticSeries(series, len).ToList();
                Assert.IsTrue(CheckSeries(ans, (int)len));

                // Should have at least 1 series which starts with i
                Assert.IsTrue(ans.Count(lst => lst[0] == (ulong) i) > 0);
            }
        }
    }
}
