using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.RepositoryInterfaces
{
    public interface IBaseBQRepository<TEntity> where TEntity : class
    {
        public TEntity GetOneByQuery(string query);
        public List<TEntity> GetAllByQuery(string query);
    }
}
