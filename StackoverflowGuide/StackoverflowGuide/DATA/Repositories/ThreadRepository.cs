using StackoverflowGuide.BLL.Models.Thread;
using StackoverflowGuide.BLL.RepositoryInterfaces;
using StackoverflowGuide.DATA.Context;

namespace StackoverflowGuide.DATA.Repositories
{
    public class ThreadRepository : BaseRepository<Thread>, IThreadRepository
    {
        public ThreadRepository(IMongoDBContext context) : base(context)
        {
        }
    }
}
