using StackoverflowGuide.BLL.Models.Post;
using StackoverflowGuide.BLL.Models.Post.Elastic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.RepositoryInterfaces
{
    public interface IQuestionsElasticRepository : IBaseElasticRepository<Question>
    {
        public Question GetById(string id);

        public List<Question> GetAllByIds(List<string> ids);

        public List<Question> SearchByText(String searchTerm, List<string> searchFields);
    }
}
