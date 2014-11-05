using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Elasticsearch.Net.Connection;

namespace ESTest
{
    public class HyperLogLogTest
    {
        public void SetUp()
        {
            try
            {
                int size = 100000;
                List<string> deviceIDs = new List<string>();
                HyperLogLog log_log = new HyperLogLog(0.02);
                for (int i = 0; i < size; i++)
                {
                    string deviceID = Guid.NewGuid().ToString();
                    deviceIDs.Add(deviceID);
                    log_log.Add(deviceID);
                }
                
                //TestErrorRate(log_log,deviceIDs);
                SaveHyperLogLog(log_log.LookupStream());
                SaveToJsonFile(deviceIDs);
                SaveToElasticSearch(deviceIDs);
                SaveHyperLogLogToES(log_log.LookupStream());

                int strLen = log_log.LookupStream().Length;
                Console.WriteLine(strLen);
            }
            catch (Exception ex)
            {
                Console.WriteLine();
            }
        }

        public void TestErrorRate(HyperLogLog log_log, List<string> deviceIDs)
        {
            //errorNew for new ID errors; errorExt for existing ID errors
            int lastCnt = log_log.Count();
            int errorExt = 0;
            int errorNew = 0;
            //test cases for existing deviceIDs
            for (int i = 0; i < 100000; i++)
            { 
                Random random = new Random(DateTime.Now.Millisecond);
                int rand = random.Next(0, deviceIDs.Count);
                string extDev = deviceIDs[rand];
                log_log.Add(extDev);
                int cnt = log_log.Count();
                if (cnt != lastCnt)
                {
                    errorExt++;
                    lastCnt = cnt;
                }
            }
            //test cases for new deviceIDs
            for (int i = 0; i < 100000; i++)
            {
                string newDev = Guid.NewGuid().ToString();
                log_log.Add(newDev);
                int cnt = log_log.Count();
                if (cnt != lastCnt)
                {
                    errorNew++;
                    lastCnt = cnt;
                }
            }


        }

        public void SaveHyperLogLogToES(byte[] dict)
        {
            var accessor = new ESDataAccessor();
            accessor.Connect();
            String base64String = Convert.ToBase64String(dict);

            var hlldoc = "{ \"device_dict\" : \""+base64String +"\" }";
            var result = accessor.Client.Index("hlltest", "hyperloglog", "1", hlldoc);
            var statusCode = result.HttpStatusCode;
     
        }

        public void GetHyperLogLogFromES()
        {
            var accessor = new ESDataAccessor();
            accessor.Connect();
            var result = accessor.Client.Get("hlltest", "hyperloglog", "1");
            var response = result.Response;

            try
            {
                var tmp = response["_source"];
                string device_dict = tmp["device_dict"];
                byte[] backToBytes = Convert.FromBase64String(device_dict);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void SaveHyperLogLog(byte[] dict)
        {
            // Set a variable to the My Documents path. 
            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            using (FileStream outfile = new FileStream(mydocpath + @"\Hyperloglog.txt", FileMode.Create,
                FileAccess.Write))
            {
                outfile.Write(dict, 0, dict.Length);
            }
        }

        public void SaveToJsonFile(List<string> deviceIds)
        {
            StringBuilder strBuilder = new StringBuilder();
            string tmpl = "{{\"device_id\":\"{0}\", \"cnt\": 1}},";

            foreach(string deviceId in deviceIds)
            {
                strBuilder.Append(string.Format(tmpl,deviceId));
            }
            strBuilder.Insert(0, '[');
            string jsonStr = strBuilder.ToString().TrimEnd(',') + ']';
            // Set a variable to the My Documents path. 
            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            using (StreamWriter outfile = new StreamWriter(mydocpath + @"\DeviceDict.json"))
            {
                outfile.Write(jsonStr);
            }
        }

        private void SaveToElasticSearch(List<string> deviceIds)
        {
            var accessor = new ESDataAccessor();
            accessor.Connect();

            var bulkList = new List<object>();
            for (int i = 0; i < deviceIds.Count; i++)
            {

               bulkList.Add(
                    new { index = new { _index = "hlltest", _type="device_dict"  }});
                bulkList.Add(new
                    {
                        device_id = deviceIds[i],
                        cnt=1
                    }
                );
            }

            var result = accessor.Client.Bulk(bulkList.ToArray());
            var statusCode = result.HttpStatusCode;
        }
    }
}
