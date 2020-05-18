using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.API.DTOs.Post
{
    public class NewPostAndSuggestionsData
    {
        public PostData NewPost { get; set; }
        public List<PostData> Suggestions { get; set; }
    }
}
