using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSQ003.ConsistentHash.Library.Tests.Libs
{
    /// <summary>
    /// Stats Helper
    /// <para>
    /// <![CDATA[https://stackoverflow.com/questions/2253874/linq-equivalent-for-standard-deviation]]>
    /// </para>
    /// <para>
    /// <![CDATA[http://warrenseen.com/blog/2006/03/13/how-to-calculate-standard-deviation/]]>
    /// </para>
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class StatsHelper
    {
        /// <summary>
        /// Standard Deviation
        /// </summary>
        /// <param name="values">list of values</param>
        /// <param name="isPopulation">True if entire population</param>
        /// <returns>StdDev </returns>
        public static double StandardDeviation(this List<double> values, bool isPopulation = true)
        {
            double avg = values.Average();
            double sum = values.Sum(v => (v - avg) * (v - avg));
            double denominator = values.Count;
            if (!isPopulation) denominator -= 1;
            return denominator > 0.0 ? Math.Sqrt(sum / denominator) : -1;
        }
    }
}
