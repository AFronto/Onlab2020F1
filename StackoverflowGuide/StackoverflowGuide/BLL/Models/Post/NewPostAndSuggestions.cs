using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.Models.Post
{
    public class NewPostAndSuggestions
    {
        public ThreadPost NewPost { get; set; }
        public List<ThreadPost> Suggestions { get; set; }
    }
}
