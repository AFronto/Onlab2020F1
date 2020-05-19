using StackoverflowGuide.BLL.BigQueryInterfaces;
using StackoverflowGuide.BLL.Models.Tag;
using StackoverflowGuide.BLL.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.DATA.Repositories
{
    public class TagBQRepository : BaseBQRepository<Tag>, ITagBQRepository
    {
        public TagBQRepository(IBigQuery bigQuery) : base(bigQuery)
        {
        }

        public List<Tag> GetAll()
        {            
            var query = $"SELECT id, tag_name FROM `bigquery-public-data.stackoverflow.tags` ";
            
            return GetAllByQuery(query);
            
        }
    }
}
