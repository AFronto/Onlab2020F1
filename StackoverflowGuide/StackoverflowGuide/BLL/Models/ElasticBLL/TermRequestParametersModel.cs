using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.Models.ElasticBLL
{
    public class TermRequestParametersModel: ElasticRequestOnFields
    {
        public string Id { get; set; }
        public int MaxNumberOfTerms { get; set; }
    }
}
