using StackoverflowGuide.BLL.Models.Post;
using StackoverflowGuide.BLL.RepositoryInterfaces;
using StackoverflowGuide.DATA.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.DATA.Repositories
{
    public class PostsRepository: BaseRepository<StoredThreadPost>, IPostsRepository
    {
        public PostsRepository(IMongoDBContext context) : base(context)
        {
        }
    }
}
