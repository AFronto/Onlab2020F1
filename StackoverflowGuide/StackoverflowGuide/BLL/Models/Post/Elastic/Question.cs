using MongoDB.Bson.Serialization.Attributes;
using StackoverflowGuide.BLL.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.Models.Post.Elastic
{
    [BsonIgnoreExtraElements]
    public class Question: ElasticModel
    {
        public long AcceptedAnswerId { get; set; }

        public long AnswerCount { get; set; }

        public string Body { get; set; }

        public long CommentCount { get; set; }

        public string CreationDate { get; set; }

        public long FavoriteCount { get; set; }

        public long Score { get; set; }

        public string Tags { get; set; }

        public string Title { get; set; }

        public long ViewCount { get; set; }
    }
}
