using MoreLinq;
using Nest;
using StackoverflowGuide.BLL.Helpers.Interfaces;
using StackoverflowGuide.BLL.Models.ElasticBLL;
using StackoverflowGuide.BLL.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.Helpers
{
    public class ElasticSuggestionHelper : IElasticSuggestionHelper
    {
        IQuestionsElasticRepository questionsElasticRepository;

        public ElasticSuggestionHelper(IQuestionsElasticRepository questionsElasticRepository)
        {
            this.questionsElasticRepository = questionsElasticRepository;
        }

        public List<string> GetCommonKeywords(List<string> questionIds)
        {
            List<ElasticKeyword> keywords = new List<ElasticKeyword>();
            List<string> fields = new List<string>() { "Body" };

            var questionsWithRawKeywords = questionIds.Select(id => questionsElasticRepository
                                                .GetTermVectorsOfDoc(new TermRequestParametersModel()
                                                {
                                                    Index = "questions",
                                                    Id = id,
                                                    Fields = fields.ToArray(),
                                                    MaxNumberOfTerms = 5
                                                })["Body"])
                                         .ToList();

            // kulcsszavak feldolgozása
            questionsWithRawKeywords.SelectMany(questionWithRawKeywords => questionWithRawKeywords
                                                .Select(rawKeyword => new ElasticKeyword(rawKeyword.Key, rawKeyword.Value.Score))
                                                .ToList())
                                    .ToList()
                                    .DistinctBy(keyword => { return keyword.Word; })
                                    
                                    ;
            foreach (IReadOnlyDictionary<string, TermVectorTerm> terms in rawKeywords)
            {
                foreach(KeyValuePair<string, TermVectorTerm> currentTerm in terms)
                {
                    ElasticKeyword currentKeyword = new ElasticKeyword(currentTerm.Key, currentTerm.Value.Score);

                    if (keywords.Contains(currentKeyword))
                    {
                        keywords.Find(kw => kw.Word.Equals(currentKeyword.Word))
                                .ExtendKeyword(currentKeyword.Score);
                    }
                    else
                    {
                        keywords.Add(currentKeyword);
                    }
                }
            }

            // közös kulcsszavak kikeresése
            // keywords.Sort<>
            // nem közös kulcsszavak rangsorolása score alapján

            // kereső kifejezés elkészítése
            // lehetséges extra paraméterek: csakKözösKulcsszavak, mennyi kulcsszóból álljon a kereső kifejezés

            throw new NotImplementedException();
        }

        public List<string> GetSuggestionIds(List<string> keywords)
        {
            throw new NotImplementedException();
        }
    }
}
