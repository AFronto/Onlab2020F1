using StackoverflowGuide.BLL.Models.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.Services.Interfaces
{
    public interface IPostService
    {
        public List<ThreadPost> GetSuggestions(string threadId);
    }
}
