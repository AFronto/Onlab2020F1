using StackoverflowGuide.BLL.Models.ElasticBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.Helpers.Interfaces
{
    public interface IElasticSuggestionHelper
    {
        public List<ElasticKeyword> GetKeywords(GetKeywordRequestParametersModel parameters);

        public List<string> GetSuggestionIds(List<string> keywords);

    }
}
