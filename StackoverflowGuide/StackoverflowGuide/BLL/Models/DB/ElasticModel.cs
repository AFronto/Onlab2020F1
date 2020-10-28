﻿using MongoDB.Bson.Serialization.Attributes;
using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.Models.DB
{
    [BsonIgnoreExtraElements]
    [ElasticsearchType(IdProperty = nameof(Id))]
    public class ElasticModel
    {
        [BsonId]
        public string Id { get; set; }
    }
}
