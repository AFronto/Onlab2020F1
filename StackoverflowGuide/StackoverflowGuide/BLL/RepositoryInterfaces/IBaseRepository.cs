using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.RepositoryInterfaces
{
    public interface IBaseRepository<TEntity> : IDisposable where TEntity : class
    {
        Task Add(TEntity obj);
        Task<TEntity> GetById(string id);
        Task<IEnumerable<TEntity>> GetAll();
        Task Update(string id, TEntity obj);
        Task Remove(string id);
    }
}
