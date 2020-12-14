using StackoverflowGuide.BLL.Models.ElasticBLL;
using StackoverflowGuide.BLL.Models.Post;
using StackoverflowGuide.BLL.Models.Post.Elastic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.Helpers.Interfaces
{
    public interface IElasticSuggestionHelper
    {
        public List<ElasticKeyword> GetKeywords(GetKeywordRequestParametersModel parameters);

        public List<Question> GetRecommendedQuestions(List<string> incomingIds, string userSearchTerm, List<string> tagsFromThread);

        public List<ThreadPost> ParseQuestionsToThreadPosts(List<Question> questions, List<StoredThreadPost> storedThreadPosts);

        public List<TagScore> GetTagScore(List<string> incomingIds, List<string> tagsFromThread);
    }
}
