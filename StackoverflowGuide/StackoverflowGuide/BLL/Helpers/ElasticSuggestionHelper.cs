using MoreLinq;
using MoreLinq.Extensions;
using Nest;
using StackoverflowGuide.BLL.Helpers.Interfaces;
using StackoverflowGuide.BLL.Models.ElasticBLL;
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
                                                    MaxNumberOfTerms = 5
                                                }).Values
                                                .SelectMany(v => v)
                                                .ToList())
                                         .ToList();

            // kulcsszavak feldolgozása
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

            // közös kulcsszavak kikeresése
            // keywords.Sort<>
            // nem közös kulcsszavak rangsorolása score alapján

            keywords = keywords.OrderByDescending(kw => kw.Occurrences).ThenBy(kw => kw.Score).ToList();

            // kereső kifejezés elkészítése
            // lehetséges extra paraméterek: csakKözösKulcsszavak, mennyi kulcsszóból álljon a kereső kifejezés

            return parameters.OnlyMultipleOccurrences ? keywords.Where(kw => kw.Occurrences > 1).ToList() : keywords;
        }

        public List<string> GetSuggestionIds(List<string> keywords)
        {
            throw new NotImplementedException();
        }
    }
}
