using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.API.DTOs.Post
{
    public class SinglePostData
    {
        public string Id { get; set; }

        public List<AnswerData> Answers { get; set; }

        public string? AcceptedAnswerId { get; set; }

        public string? Body { get; set; }

        public string CreationDate { get; set; }

        public long? FavoriteCount { get; set; }

        public long Score { get; set; }

        public string? Tags { get; set; }

        public string? Title { get; set; }

        public long? ViewCount { get; set; }
    }
}
