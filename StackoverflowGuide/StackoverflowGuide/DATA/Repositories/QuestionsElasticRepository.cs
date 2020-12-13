using Nest;
using StackoverflowGuide.BLL.Models.ElasticBLL;
using StackoverflowGuide.BLL.Models.Post;
using StackoverflowGuide.BLL.Models.Post.Elastic;
using StackoverflowGuide.BLL.Models.Tag;
using StackoverflowGuide.BLL.RepositoryInterfaces;
using StackoverflowGuide.DATA.Context.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.DATA.Repositories
{
    public class QuestionsElasticRepository: BaseElasticRepository<Question>, IQuestionsElasticRepository
    {
        public QuestionsElasticRepository(IElasticStackContext elastic) : base(elastic)
        {
        }

        public List<Question> GetAllByIds(List<string> ids)
        {
            var query = new SearchRequest<Question>(Nest.Indices.Index("questions"))
            {
                Size = ids.Count,
                Query = new IdsQuery
                {
                    Name = "named_query",
                    Values = ids.Select(id => new Id(id)).ToList()
                }
            };

            if (ids.Count > 0)
            {
                return SearchByQuery(query);
            }
            else
            {
                return new List<Question>();
            }
        }

        public Question GetById(string id)
        {
            var ret = GetAllByIds(new List<string> { id });
            return ret[0];
        }

        public List<Question> SearchByText(string searchTerm, List<string> searchFields, List<string> idsToNotSearch)
        {
            var query = new SearchRequest<Question>(Nest.Indices.Index("questions"))
            {
                Size = 10,
                Query = new MultiMatchQuery
                {
                    Fields = searchFields.ToArray(),
                    Query = searchTerm
                } &&
                !new IdsQuery
                {
                    Values = idsToNotSearch.Select(id => new Id(id)).ToList()
                }
            };

            return SearchByQuery(query);
        }

        public Dictionary<string, IReadOnlyDictionary<string, TermVectorTerm>> GetTermVectorsOfDoc(TermRequestParametersModel requestParameters)
        {
            TermVectorsDescriptor<Question> termVectorDescriptor = new TermVectorsDescriptor<Question>();

            termVectorDescriptor.Index(requestParameters.Index);
            termVectorDescriptor.Id(requestParameters.Id);
            termVectorDescriptor.Fields(requestParameters.Fields);
            termVectorDescriptor.TermStatistics(true);
            termVectorDescriptor.Offsets(false);
            termVectorDescriptor.Positions(false);
            termVectorDescriptor.Payloads(false);
            termVectorDescriptor.Filter(f => f.MaximimumNumberOfTerms(requestParameters.MaxNumberOfTerms));

            var termVectorResponse = TermRequestToDoc(termVectorDescriptor);

            return termVectorResponse.TermVectors
                                     .Select(tvItem => new KeyValuePair<string, IReadOnlyDictionary<string, TermVectorTerm>>(tvItem.Key.Name,
                                                                                                                             tvItem.Value.Terms))
                                     .ToDictionary(tv => tv.Key, tv => tv.Value);
        }

        public List<DbTag> GetAllTags()
        {
            var query = new SearchRequest<Question>(Nest.Indices.Index("questions"))
            {
                Size = 0,
                Aggregations = new TermsAggregation("tag_agg")
                {
                    Field = "Tags",
                    Size = 10000000
                }
            };

            return SingleAggregateByQuery(query).Select(tag => new DbTag() { Id="elastic-tag", Name=tag}).ToList();
        }
    }
}
