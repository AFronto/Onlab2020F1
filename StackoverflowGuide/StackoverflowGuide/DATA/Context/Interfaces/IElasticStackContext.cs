using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.DATA.Context.Interfaces
{
    public interface IElasticStackContext
    {
        public ElasticClient client { get; set; }
    }
}
