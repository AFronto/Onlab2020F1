using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.API.DTOs.Post
{
    public class PostData
    {
        public string Id { get; set; }
        public int ThreadIndex { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public List<string> ConnectedPosts { get; set; }
    }
}
