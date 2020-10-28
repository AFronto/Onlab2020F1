using Nest;
using StackoverflowGuide.DATA.Context.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.DATA.Context
{
    public class ElasticStackContext: IElasticStackContext
    {
        public ElasticClient client { get; set; }

        public ElasticStackContext()
        {
            var node = new Uri("http://localhost:9200");
            var settings = new ConnectionSettings(node);
            settings.DefaultFieldNameInferrer(p => p);
            client = new ElasticClient();
        }
    }
}
