using StackoverflowGuide.BLL.Models.Tag;
using StackoverflowGuide.BLL.RepositoryInterfaces;
using StackoverflowGuide.DATA.Context;


namespace StackoverflowGuide.DATA.Repositories
{
    public class TagRepository : BaseRepository<DbTag>, ITagRepository
    {
        public TagRepository(IMongoDBContext context) : base(context)
        {
        }
    }
}
