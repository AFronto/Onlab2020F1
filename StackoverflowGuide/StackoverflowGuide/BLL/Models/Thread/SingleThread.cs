using StackoverflowGuide.BLL.Models.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.Models.Thread
{
    public class SingleThread
    {
        public Thread Thread { get; set; }
        public List<ThreadPost> Posts { get; set; }
    }
}
