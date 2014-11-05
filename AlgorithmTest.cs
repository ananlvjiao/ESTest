using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESTest
{
    public class AlgorithmTest
    {
        public void SetUp()
        {
            StringBuilder strBuilder = new StringBuilder();
            for (double i = 0.01; i < 0.06; )
            {
                double stdErr = i;

                var algTestResult = GetErrorRate(stdErr, 2, 6, 10);
                strBuilder.AppendLine("For StdErr " + stdErr);
                strBuilder.AppendLine("The map size (m) is :" + GetMapSize(stdErr));
                foreach (var errRateStat in algTestResult)
                {
                    strBuilder.AppendLine(errRateStat.GetSummary());
                }
                strBuilder.AppendLine();
                i = i + 0.01;
            }

            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            using (StreamWriter outfile = new StreamWriter(mydocpath + @"\HyperLogLogTestResult.text"))
            {
                outfile.Write(strBuilder);
            }
        }

        public double GetMapSize(double stdError)
        {
            var log_log = new HyperLogLog(stdError);
            return log_log.GetMapSize();        
        }
        
        /// <summary>
        /// Get the error rate stats (min, max, avg) of x times for N in (10e+minE - 10e+ maxE)
        /// </summary>
        /// <param name="stdError">define the standard Error (relative accuracy)</param>
        /// <param name="minE">10e+minE cardinality</param>
        /// <param name="maxE">10e+maxE cardinality</param>
        /// <param name="times">exp times to get error rate stats (min, max, avg)</param>
        /// <returns></returns>
        public List<ErrorRateStats> GetErrorRate(double stdError, int minE, int maxE, int times)
        {
            var errorRateStats = new List<ErrorRateStats>();
            for (int e = minE; e <= maxE; e++)
            {
                double size = Math.Pow(10, e);
                var errRateStat = GetErrorRateStats(times, stdError, size);
                errorRateStats.Add(errRateStat);
            }

            return errorRateStats;
        }

        public ErrorRateStats GetErrorRateStats(int times, double stdError, double size)
        {
            var errorRates = new List<double>();
            for (int m = 0; m < times; m++)
            {
                var log_log = new HyperLogLog(stdError);
                for (int i = 0; i < size; i++)
                {
                    string unique = Guid.NewGuid().ToString();
                    log_log.Add(unique);
                }
                double predSize = log_log.Count();
                double errorRate = Math.Abs(predSize - size) / size;
                errorRates.Add(errorRate);
            }
            var errorRateStats = new ErrorRateStats(size,errorRates);
            return errorRateStats;
        }


    }

    
}
