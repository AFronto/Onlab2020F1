using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.Models.ElasticBLL
{
    public class GetKeywordRequestParametersModel: ElasticRequestOnFields
    {
        public List<string> QuestionIds { get; set; }
        public bool OnlyMultipleOccurrences { get; set; }
    }
}
