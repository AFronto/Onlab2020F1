using StackoverflowGuide.BLL.Models.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.RepositoryInterfaces
{
    public interface IPostsBQRepository: IBaseBQRepository<BQPost>
    {
        public BQPost GetById(string id);

        public List<BQPost> GetAllByIds(List<string> ids);

    }
}
