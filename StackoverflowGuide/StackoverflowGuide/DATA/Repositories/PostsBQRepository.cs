using StackoverflowGuide.BLL.BigQueryInterfaces;
using StackoverflowGuide.BLL.Models.Post;
using StackoverflowGuide.BLL.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.DATA.Repositories
{
    public class PostsBQRepository: BaseBQRepository<Post>, IPostsBQRepository
    {
        public PostsBQRepository(IBigQuery bigQuery) : base(bigQuery)
        {
        }

        public List<Post> GetAllByIds(List<string> ids)
        {
            var query = $"SELECT id, title, body FROM `bigquery-public-data.stackoverflow.posts_questions` a WHERE a.id in ({string.Join(",", ids.ToArray())})";
            if(ids.Count > 0)
            {
                return GetAllByQuery(query);
            }
            else
            {
                return new List<Post>();
            }
        }

        public Post GetById(string id)
        {
            var query = $"SELECT id, title, body FROM `bigquery-public-data.stackoverflow.posts_questions` a WHERE a.id = {id}";
            return GetOneByQuery(query);
        }
    }
}
