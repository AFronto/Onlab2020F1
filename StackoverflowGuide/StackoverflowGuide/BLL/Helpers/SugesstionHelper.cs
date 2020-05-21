using Google.Apis.Bigquery.v2.Data;
using MongoDB.Bson;
using MoreLinq;
using StackoverflowGuide.API.DTOs.Thread;
using StackoverflowGuide.BLL.Helpers.Interfaces;
using StackoverflowGuide.BLL.Helpers.Models;
using StackoverflowGuide.BLL.Models.Post;
using StackoverflowGuide.BLL.Models.Tag;
using StackoverflowGuide.BLL.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.Helpers
{
    public class SugesstionHelper : ISuggestionHelper
    {
        private ITagRepository tagRepository;
        private Dictionary<int, SuggestedPost> postInClusters = new Dictionary<int, SuggestedPost>();
        public SugesstionHelper(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
            CreateTagRepository();
            ReadHierarchy();
        }
        public List<string> GetSuggestionIds(List<string> incomingIds)
        {

            var buffedIncoming = incomingIds.Concat(new List<string> { "4", "25224", "78669", "75267", "19" })
                                            .Distinct()
                                            .Take(5);


            var posts = GetPostsFromCluster(GetCommonCluster(buffedIncoming
                            .Select(id => Int32.Parse(id)).ToList()));
            Random rng = new Random();
            var shuffledPosts = posts.OrderBy(a => rng.Next());
            return shuffledPosts.Where(p => !incomingIds.Contains("" + p) && !buffedIncoming.Contains("" + p))
                                .Take(3)
                                .Select(id => id.ToString())
                                .ToList();

        }

        public List<ThreadPost> ParseSuggestions(List<Post> bqPosts, List<StoredThreadPost> storedThreadPosts)
        {
            return bqPosts.Select(bqP => new ThreadPost
            {
                Id = bqP.Id,
                Title = bqP.Title,
                Body = bqP.Body,
                ThreadIndex = -1,
                ConnectedPosts = storedThreadPosts.Count() > 0
                                    ?
                                    new List<string> { storedThreadPosts.MaxBy(sTP => sTP.ThreadIndex).First().ThreadId }
                                    :
                                    new List<string>()
            }
            ).ToList();
        }

        public List<int> GetPostsFromCluster(int clusterId)
        {
            return postInClusters.Where(postAndClusters => postAndClusters.Value.Clusters.Contains(clusterId))
                                 .Select(postAndClusters => postAndClusters.Key)
                                 .ToList();
        }


        private int GetCommonCluster(List<int> ids)
        {
            List<int> commonClusters = postInClusters[ids.First()].Clusters;
            foreach (int id in ids)
            {
                commonClusters = commonClusters.Intersect(postInClusters[id].Clusters).ToList();
            }

            return commonClusters.First();
        }

        private void ReadHierarchy()
        {
            List<int> postIds = new List<int>();
            List<List<int>> listOfMerges = new List<List<int>>();
            Dictionary<int, List<string>> tagsOfPosts = new Dictionary<int, List<string>>();

            using (var parser = new Microsoft.VisualBasic.FileIO.TextFieldParser("Resources/linkage.csv"))
            {
                parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                parser.SetDelimiters(new string[] { ";" });

                while (!parser.EndOfData)
                {
                    string[] row = parser.ReadFields();
                    List<int> merge = new List<int>();
                    merge.Add(int.Parse(row[0]));
                    merge.Add(int.Parse(row[1]));
                    listOfMerges.Add(merge);
                }
            }

            using (var parser = new Microsoft.VisualBasic.FileIO.TextFieldParser("Resources/id.csv"))
            {
                parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                parser.SetDelimiters(new string[] { ";" });

                while (!parser.EndOfData)
                {
                    string[] row = parser.ReadFields();
                    postIds.Add(int.Parse(row[0]));
                }
            }

            using (var parser = new Microsoft.VisualBasic.FileIO.TextFieldParser("Resources/tag.csv"))
            {
                parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                parser.SetDelimiters(new string[] { ";" });

                while (!parser.EndOfData)
                {
                    string[] row = parser.ReadFields();
                    string[] splitTags = row[2].Split(',');
                    List<string> tags = new List<string>();
                    foreach (string splitTag in splitTags)
                    {
                        if(splitTag != "")
                            tags.Add(splitTag);
                    }
                    tagsOfPosts.Add(int.Parse(row[1]), tags);
                }
            }

            for (int i = 0; i < postIds.Count; i++)
            {
                postInClusters.Add(postIds[i], new SuggestedPost());

                List<int> idHit = listOfMerges.Where(pair => pair.Contains(i)).First();
                var clusterNumber = listOfMerges.IndexOf(idHit) + postIds.Count;
                postInClusters[postIds[i]].Clusters.Add(clusterNumber);
                while (clusterNumber != (postIds.Count - 1) * 2)
                {
                    idHit = listOfMerges.Where(pair => pair.Contains(clusterNumber)).First();
                    clusterNumber = listOfMerges.IndexOf(idHit) + postIds.Count;
                    postInClusters[postIds[i]].Clusters.Add(clusterNumber);
                    postInClusters[postIds[i]].TagList = tagsOfPosts[postIds[i]];
                }
            }
        }

        private void CreateTagRepository()
        {
            tagRepository.EmptyTable();

            using (var parser = new Microsoft.VisualBasic.FileIO.TextFieldParser("Resources/uniqueTag.csv"))
            {
                parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                parser.SetDelimiters(new string[] { ";" });

                while (!parser.EndOfData)
                {
                    string[] row = parser.ReadFields();
                    var id = ObjectId.GenerateNewId().ToString();
                    DbTag tag = new DbTag();
                    tag.Id = id;
                    tag.Name = row[1];
                    if (!tagRepository.Create(tag))
                    {
                        throw new Exception("Couldn't create a new Tag!");
                    }
                }
            }
        }
    }
}
