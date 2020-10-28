﻿using Nest;
using StackoverflowGuide.BLL.Models.Post;
using StackoverflowGuide.BLL.Models.Post.Elastic;
using StackoverflowGuide.BLL.RepositoryInterfaces;
using StackoverflowGuide.DATA.Context.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.DATA.Repositories
{
    public class QuestionsElasticRepository: BaseElasticRepository<Question>, IQuestionsElasticRepository
    {
        public QuestionsElasticRepository(IElasticStackContext elastic) : base(elastic)
        {
        }

        public List<Question> GetAllByIds(List<string> ids)
        {
            var query = new SearchRequest<Question>(Nest.Indices.Index("questions"))
            {
                Size = ids.Count,
                Query = new IdsQuery
                {
                    Name = "named_query",
                    Values = ids.Select(id => new Id(id)).ToList()
                }
            };

            if (ids.Count > 0)
            {
                var ret = SearchByQuery(query);
                return new List<Question>();
            }
            else
            {
                return new List<Question>();
            }
        }

        public Question GetById(string id)
        {
            var ret = GetAllByIds(new List<string> { id });
            return new Question();
        }

    }
}
