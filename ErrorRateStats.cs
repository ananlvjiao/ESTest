using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESTest
{
    public class ErrorRateStats
    {
        private double size;
        public double MinErrorRate { get; set; }
        public double MaxErrorRate { get; set; }
        public double AvgErrorRate { get; set; }

        public ErrorRateStats(double size, List<double> errorRates)
        {
            this.size = size;
            MinErrorRate = errorRates.Min();
            MaxErrorRate = errorRates.Max();
            AvgErrorRate = errorRates.Average();
        }

        public string GetSummary()
        {
            string summary = string.Format("For {0} cardinality, Average error rate: {1},"
                            +"Minimum error rate: {2}, Maximum error rate: {3}", size, AvgErrorRate,
                            MinErrorRate, MaxErrorRate );
            return summary;
        }
    }
}
