using Nest;
using StackoverflowGuide.BLL.Models.Post.Elastic;
using StackoverflowGuide.BLL.RepositoryInterfaces;
using StackoverflowGuide.DATA.Context.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.DATA.Repositories
{
    public class AnswerElasticRepository : BaseElasticRepository<Answer>, IAnswerElasticRepository
    {
        public AnswerElasticRepository(IElasticStackContext elastic) : base(elastic)
        {
        }

        public List<Answer> GetAllByQuestionId(string questionId)
        {
            var query = new SearchRequest<Answer>(Nest.Indices.Index("answers"))
            {
                Query = new MatchQuery
                {
                    Name = "named_query",
                    Field = "ParentId",
                    Query = questionId
                }
            };

            
            return SearchByQuery(query);         
        }
    }
}
