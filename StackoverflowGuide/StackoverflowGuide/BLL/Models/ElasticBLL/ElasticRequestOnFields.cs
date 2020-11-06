using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.Models.ElasticBLL
{
    public class ElasticRequestOnFields
    {
        public string[] Fields { get; set; }
        public string Index { get; set; }
    }
}
