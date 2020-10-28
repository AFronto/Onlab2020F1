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

        public TEntity SearchByQuery(Nest.SearchRequest<TEntity> query)
        {
            var searchResponse = elastic.client.Search<TEntity>(query);
            return new TEntity();
        }
    }
}
