using StackoverflowGuide.BLL.Models.Post.Elastic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.RepositoryInterfaces
{
    public interface IAnswerElasticRepository : IBaseElasticRepository<Answer>
    {
        public List<Answer> GetAllByQuestionId(string questionId);
    }
}
