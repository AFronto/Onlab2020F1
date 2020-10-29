using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.Helpers.Interfaces
{
    interface IElasticSuggestionHelper
    {
        public List<string> GetSuggestionIds(List<string> keywords);

        public List<string> GetCommonKeywords(List<string> questionIds);


    }
}
