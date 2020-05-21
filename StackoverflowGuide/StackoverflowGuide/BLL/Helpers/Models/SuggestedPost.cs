using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.Helpers.Models
{
    public class SuggestedPost
    {
        public List<int> Clusters { get; set; }
        public List<string> TagList { get; set; }

        public SuggestedPost()
        {
            Clusters = new List<int>();
            TagList = new List<string>();
        }
    }
}
