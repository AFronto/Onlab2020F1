﻿using MoreLinq;
using Nest;
using StackoverflowGuide.BLL.Helpers.Interfaces;
using StackoverflowGuide.BLL.Models.ElasticBLL;
using StackoverflowGuide.BLL.Models.Post;
using StackoverflowGuide.BLL.Models.Post.Elastic;
using StackoverflowGuide.BLL.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StackoverflowGuide.BLL.Helpers
{
    public class ElasticSuggestionHelper : IElasticSuggestionHelper
    {
        IQuestionsElasticRepository questionsElasticRepository;

        public ElasticSuggestionHelper(IQuestionsElasticRepository questionsElasticRepository)
        {
            this.questionsElasticRepository = questionsElasticRepository;
        }

        public List<ElasticKeyword> GetKeywords(GetKeywordRequestParametersModel parameters)
        {
            var questionsWithRawKeywords = parameters.QuestionIds.Select(id => questionsElasticRepository
                                                .GetTermVectorsOfDoc(new TermRequestParametersModel()
                                                {
                                                    Index = parameters.Index,
                                                    Id = id,
                                                    Fields = parameters.Fields,
                                                    MaxNumberOfTerms = 2
                                                }).Values
                                                .SelectMany(v => v)
                                                .ToList())
                                         .ToList();

            var keywords = questionsWithRawKeywords
                                    .SelectMany(questionWithRawKeywords => questionWithRawKeywords
                                                .Select(rawKeyword => new ElasticKeyword()
                                                {
                                                    Word = rawKeyword.Key,
                                                    Occurrences = 1,
                                                    Score = rawKeyword.Value.Score
                                                })
                                                .ToList())
                                    .ToList()
                                    .GroupBy(keyword => keyword.Word,
                                             keyword => keyword,
                                             (word, groupedKeywords) => new ElasticKeyword()
                                             {
                                                 Word = word,
                                                 Occurrences = groupedKeywords.Count(),
                                                 Score = groupedKeywords.Sum(kw => kw.Score)
                                             })
                                    .ToList();

            keywords = keywords.OrderByDescending(kw => kw.Occurrences).ThenByDescending(kw => kw.Score).ToList();

            return parameters.OnlyMultipleOccurrences ? keywords.Where(kw => kw.Occurrences > 1).ToList() : keywords;
        }

        public List<Question> GetRecommendedQuestions(List<string> incomingIds, string userSearchTerm, List<string> tagsFromThread)
        {
            string searchTerm = userSearchTerm;
            if (incomingIds.Count == 0 && searchTerm == "")
            {
                searchTerm = String.Join(',', tagsFromThread);
            }

            List<TagScore> incomingTagScores = new List<TagScore>();
            if (incomingIds.Count != 0)
            {
                incomingTagScores = GetTagScore(incomingIds, tagsFromThread);

                string[] f = { "Body" };
                List<ElasticKeyword> commonKeywords = GetKeywords(new GetKeywordRequestParametersModel()
                {
                    Index = "questions",
                    Fields = f,
                    QuestionIds = incomingIds,
                    OnlyMultipleOccurrences = false
                });
                searchTerm = String.Join(',', searchTerm, String.Join(',', commonKeywords.Select(cKw => cKw.Word).ToList()));
            }


            List<string> searchFields = new List<string>() { "Body" };
            List<Question> ret = questionsElasticRepository.SearchByText(searchTerm, searchFields, incomingIds);
            var orderedByTagsQuestions = ret.Select(question => new KeyValuePair<int, Question>(CalculateQuestionsTagScore(incomingTagScores, question), question))
                                .OrderByDescending(questionKV => questionKV.Key )
                                .Select(questionKV => questionKV.Value)
                                .ToList();
            return orderedByTagsQuestions.Take(3).ToList();

            
        }

        public List<ThreadPost> ParseQuestionsToThreadPosts(List<Question> questions, List<StoredThreadPost> storedThreadPosts)
        {
            return questions.Select(q => new ThreadPost
            {
                Id = q.Id,
                Title = q.Title,
                Body = q.Body,
                ThreadIndex = -1,
                ConnectedPosts = storedThreadPosts.Count() > 0
                                    ?
                                    new List<string> { storedThreadPosts.MaxBy(sTP => sTP.ThreadIndex).First().Id }
                                    :
                                    new List<string>()
            }
            ).ToList();
        }

        public List<TagScore> GetTagScore(List<string> incomingIds, List<string> tagsFromThread)
        {
            List<Question> Questions = questionsElasticRepository.GetAllByIds(incomingIds);

            List<string> splitTags = new List<string>();
            splitTags.AddRange(tagsFromThread);
            Questions.ForEach(question => splitTags.AddRange(question.Tags.Substring(1, question.Tags.Length - 2).Split("><").ToList()));

            List<TagScore> tags = splitTags.Select(splitTag => new TagScore() { Tag = splitTag, Occurrences = 1,})
                                           .ToList()
                                           .GroupBy(tag => tag.Tag,
                                                     tag => tag,
                                                     (tag, groupedTags) => new TagScore()
                                                     {
                                                         Tag = tag,
                                                         Occurrences = groupedTags.Count(),
                                                     })
                                           .ToList();
            return tags;
        }

        private int CalculateQuestionsTagScore(List<TagScore> tags, Question question)
        {
            int score = 0;

            List<string> questionTags = question.Tags.Substring(1, question.Tags.Length - 2).Split("><").ToList();
            questionTags.ForEach(qTag => tags.Where(t => t.Tag == qTag).ForEach(t => score += t.Occurrences));

            return score;
        }

    }
}
