using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Elasticsearch.Net.Connection;

namespace ESTest
{
    public class ESDataAccessor
    {
        public ElasticsearchClient Client { get; set; }
        //connection pool or singleton later...
        public void Connect()
        {
            var node = new Uri("http://192.168.0.13:9200");
            var config = new ConnectionConfiguration(node);
            Client = new ElasticsearchClient(config);
        }

      
    }
}
