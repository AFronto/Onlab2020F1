using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.RepositoryInterfaces
{
    public interface IBaseElasticRepository<TEntity> where TEntity : class
    {
        public TEntity SearchByQuery(SearchRequest<TEntity> query);
    }
}
