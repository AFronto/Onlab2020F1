using StackoverflowGuide.BLL.Models.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.RepositoryInterfaces
{
    public interface IPostsBQRepository: IBaseBQRepository<Post>
    {
        public Post GetById(string id);

        public List<Post> GetAllByIds(List<string> ids);

    }
}
