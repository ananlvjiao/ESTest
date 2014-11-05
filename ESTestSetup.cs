using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ESTest
{
    public class ESTestSetup
    {
        public void SetUp()
        {
            try
            {
                var accessor = new ESDataAccessor();
                accessor.Connect();
                for (int j = 0; j < 5; j++)
                {
                    StringBuilder strBuilder = new StringBuilder();
                    string templ = @"{""script"" : ""ctx._source.normal_dictionary.device_dict += devices"",
                                    ""params"" : {""devices"": [";
                    strBuilder.Append(templ);
                    int len = 50000;
                    for (int i = 0; i < len; i++)
                    {
                        strBuilder.Append("{");
                        strBuilder.Append(string.Format("\"device_id\" : \"{0}\", \"cnt\" : {1}",
                            Guid.NewGuid().ToString(), 1));
                        strBuilder.Append("}");
                        if (i != len - 1)
                        {
                            strBuilder.Append(",");
                        }
                    }
                    strBuilder.Append("]}}");
                    string body = strBuilder.ToString();

                    var result = accessor.Client.Update("estest", "normal_dictionary", "1", body);
                    var statusCode = result.HttpStatusCode;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
