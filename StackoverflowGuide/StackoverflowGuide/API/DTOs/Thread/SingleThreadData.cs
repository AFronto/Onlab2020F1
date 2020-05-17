using StackoverflowGuide.API.DTOs.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.API.DTOs.Thread
{
    public class SingleThreadData
    {
        public ThreadData Thread { get; set; }
        public List<PostData> Posts { get; set; }
        public List<PostData> Suggestions { get; set; }
    }
}
