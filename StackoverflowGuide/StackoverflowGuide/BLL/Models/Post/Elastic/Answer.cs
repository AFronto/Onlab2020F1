using StackoverflowGuide.BLL.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.Models.Post.Elastic
{
    public class Answer: ElasticModel
    {
        public string? Body { get; set; }
        public long? CommentCount { get; set; }
        public string CreationDate { get; set; }
        public long ParentId { get; set; }
        public long Score { get; set; }
    }
}
