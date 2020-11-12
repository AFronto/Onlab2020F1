using MongoDB.Bson;
using MoreLinq;
using StackoverflowGuide.BLL.Helpers.Interfaces;
using StackoverflowGuide.BLL.Models.Post;
using StackoverflowGuide.BLL.Models.Tag;
using StackoverflowGuide.BLL.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;


namespace StackoverflowGuide.BLL.Helpers
{
    public class BQSuggestionHelper : IBQSuggestionHelper
    {
        private ITagRepository tagRepository;
        private IPostInClusterRepository postInClusterRepository;

        public BQSuggestionHelper(ITagRepository tagRepository, IPostInClusterRepository postInClusterRepository)
        {
            this.tagRepository = tagRepository;
            this.postInClusterRepository = postInClusterRepository;
            while (true)
            {
                Console.WriteLine("Want to load the suggestions data from files to the database? (y/n)");
                string answer = Console.ReadLine();
                answer.ToLower();
                if (answer.Length > 0 && answer[0] == 'y')
                {
                    Console.WriteLine("Loading...");
                    CreateTagRepository();
                    ReadHierarchy();
                    break;
                }
                else if (answer.Length > 0 && answer[0] == 'n')
                {
                    break;
                }
            }


        }
        public List<string> GetSuggestionIds(List<string> incomingIds, List<string> tagsFromThread)
        {

            var mostImportantIncoming = incomingIds.Take(5).Select(id => Int32.Parse(id)).ToList();
            int highestCluster = Convert.ToInt32((postInClusterRepository.Count() - 1) * 2);

            var allPostFromCluster = GetPostsFromCluster(mostImportantIncoming.Count() < 5
                                            ? highestCluster
                                            : GetCommonCluster(mostImportantIncoming));

            var mostImportantIncomingPost = postInClusterRepository.Querry(p => mostImportantIncoming.Contains(p.PostId)).ToList();

            var posts = allPostFromCluster.Keys.ToList();

            var orderedPosts = posts.OrderByDescending(postId =>
            {
                var tagMatchCountList = mostImportantIncomingPost
                       .Select(mII => mII.TagList
                       .Intersect(allPostFromCluster[postId])
                       .Count()).ToList();
                tagMatchCountList.Add(tagsFromThread.Intersect(allPostFromCluster[postId])
                                                    .Count());
                return tagMatchCountList.Sum(x => x);
            });

            return orderedPosts.Where(p => !incomingIds.Contains("" + p))
                                .Take(3)
                                .Select(id => id.ToString())
                                .ToList();

        }

        public List<ThreadPost> ParseSuggestions(List<BQPost> bqPosts, List<StoredThreadPost> storedThreadPosts)
        {
            return bqPosts.Select(bqP => new ThreadPost
            {
                Id = bqP.Id,
                Title = bqP.Title,
                Body = bqP.Body,
                ThreadIndex = -1,
                ConnectedPosts = storedThreadPosts.Count() > 0
                                    ?
                                    new List<string> { storedThreadPosts.MaxBy(sTP => sTP.ThreadIndex).First().Id }
                                    :
                                    new List<string>()
            }
            ).ToList();
        }

        public Dictionary<int, List<String>> GetPostsFromCluster(int clusterId)
        {
            return postInClusterRepository.Querry(pIC => pIC.Clusters.Contains(clusterId))
                                          .ToDictionary(p => p.PostId, p => p.TagList);
        }


        private int GetCommonCluster(List<int> ids)
        {
            var postInClustersList = postInClusterRepository.Querry(pIC => ids.Contains(pIC.PostId)).ToList();

            List<int> commonClusters = postInClustersList.First().Clusters;
            foreach (var postInCluster in postInClustersList)
            {
                commonClusters = commonClusters.Intersect(postInCluster.Clusters).ToList();
            }

            return commonClusters.First();
        }

        private void ReadHierarchy()
        {
            postInClusterRepository.EmptyTable();
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
                        if (splitTag != "")
                            tags.Add(splitTag);
                    }
                    tagsOfPosts.Add(int.Parse(row[1]), tags);
                }
            }

            for (int i = 0; i < postIds.Count(); i++)
            {
                var postInCluster = new PostInCluster
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    PostId = postIds[i],
                    Clusters = new List<int>(),
                    TagList = new List<string>()
                };

                List<int> idHit = listOfMerges.Where(pair => pair.Contains(i)).First();
                var clusterNumber = listOfMerges.IndexOf(idHit) + postIds.Count();
                postInCluster.Clusters.Add(clusterNumber);
                while (clusterNumber != (postIds.Count() - 1) * 2)
                {
                    idHit = listOfMerges.Where(pair => pair.Contains(clusterNumber)).First();
                    clusterNumber = listOfMerges.IndexOf(idHit) + postIds.Count();
                    postInCluster.Clusters.Add(clusterNumber);
                    postInCluster.TagList = tagsOfPosts[postIds[i]];
                }

                postInClusterRepository.Create(postInCluster);
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
