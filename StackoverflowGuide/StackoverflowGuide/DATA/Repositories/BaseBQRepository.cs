using StackoverflowGuide.BLL.Models.DB;
using StackoverflowGuide.BLL.RepositoryInterfaces;
using StackoverflowGuide.DATA.BigQuery.BigQueryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.DATA.Repositories
{
    public class BaseBQRepository<TEntity> : IBaseBQRepository<TEntity> where TEntity : BQModel, new()
    {
        private IBigQuery bigQuery;
        public BaseBQRepository(IBigQuery bigQuery)
        {
            this.bigQuery = bigQuery;
        }

        public TEntity GetOneByQuery(string query)
        {
            var rows = bigQuery.GetRows(query);
            var result = new TEntity();
            result.FromRow(rows.First());
            return result;
        }

        public List<TEntity> GetAllByQuery(string query)
        {
            var rows = bigQuery.GetRows(query);
            return rows.Select(r =>
            {
                var element = new TEntity();
                element.FromRow(r);
                return element;
            }).ToList();
        }
    }

}
