using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.RepositoryInterfaces
{
    public interface IBaseElasticRepository<TEntity> where TEntity : class
    {
        public List<TEntity> SearchByQuery(SearchRequest<TEntity> query);

        public List<string> SingleAggregateByQuery(SearchRequest<TEntity> query);

        public TermVectorsResponse TermRequestToDoc(TermVectorsDescriptor<TEntity> request);
    }
}
