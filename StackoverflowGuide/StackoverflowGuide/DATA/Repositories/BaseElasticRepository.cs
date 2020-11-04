using Elasticsearch.Net;
using MoreLinq;
using MoreLinq.Extensions;
using Nest;
using StackoverflowGuide.BLL.Models.DB;
using StackoverflowGuide.BLL.RepositoryInterfaces;
using StackoverflowGuide.DATA.Context;
using StackoverflowGuide.DATA.Context.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.DATA.Repositories
{
    public class BaseElasticRepository<TEntity> : IBaseElasticRepository<TEntity> where TEntity : ElasticModel, new()
    {
        private IElasticStackContext elastic;

        public BaseElasticRepository(IElasticStackContext elastic)
        {
            this.elastic = elastic;
        }

        public TermVectorsResponse TermRequestToDoc(TermVectorsDescriptor<TEntity> requestParameters)
        {
            return elastic.client.TermVectors(requestParameters);
        }

        public List<TEntity> SearchByQuery(Nest.SearchRequest<TEntity> query)
        {
            var searchResponse = elastic.client.Search<TEntity>(query);
            return searchResponse.Hits.Select(h => {
                                              h.Source.Id = h.Id;
                                              return h.Source; })
                                      .ToList();
        }
    }
}
