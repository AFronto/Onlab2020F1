using StackoverflowGuide.BLL.Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.Helpers
{
    public class ElasticSuggestionHelper : IElasticSuggestionHelper
    {
        public List<string> GetCommonKeywords(List<string> questionIds)
        {
            throw new NotImplementedException();
        }

        public List<string> GetSuggestionIds(List<string> keywords)
        {
            throw new NotImplementedException();
        }
    }
}
