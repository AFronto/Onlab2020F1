using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.API.DTOs.Post
{
    public class AnswerData
    {
        public string Id { get; set; }
        public string? Body { get; set; }
        public string CreationDate { get; set; }
        public long Score { get; set; }
    }
}
