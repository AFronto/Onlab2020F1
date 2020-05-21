using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.RepositoryInterfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        TEntity Find(string id);

        bool Update(TEntity model);

        bool Create(TEntity model);

        bool Delete(string id);

        DeleteResult EmptyTable();

        IEnumerable<TEntity> QuerryAll();

        IEnumerable<TEntity> Querry(Expression<Func<TEntity,bool>> filter);
    }
}
