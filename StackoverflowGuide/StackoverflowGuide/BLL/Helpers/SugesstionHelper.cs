using Google.Apis.Bigquery.v2.Data;
using MoreLinq;
using StackoverflowGuide.API.DTOs.Thread;
using StackoverflowGuide.BLL.Helpers.Interfaces;
using StackoverflowGuide.BLL.Models.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.Helpers
{
    public class SugesstionHelper : ISuggestionHelper
    {

        private Dictionary<int, List<int>> postInClusters = new Dictionary<int, List<int>>();
        public SugesstionHelper()
        {
            ReadHierarchy();
        }
        public List<string> GetSuggestionIds(List<string> incomingIds)
        {

            var buffedIncoming = incomingIds.Concat(new List<string> { "13", "192", "78669", "75267", "19" })
                                            .Distinct()
                                            .Take(5);


            var posts = GetPostsFromCluster(GetCommonCluster(buffedIncoming
                            .Select(id => Int32.Parse(id)).ToList()));
            return posts.Take(3).Select(id => id.ToString()).ToList();

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
            return postInClusters.Where(postAndClusters => postAndClusters.Value.Contains(clusterId))
                                 .Select(postAndClusters => postAndClusters.Key)
                                 .ToList();
        }


        private int GetCommonCluster(List<int> ids)
        {
            List<int> commonClusters = postInClusters[ids.First()];
            foreach (int id in ids)
            {
                commonClusters = commonClusters.Intersect(postInClusters[id]).ToList();
            }

            return commonClusters.First();
        }

        private void ReadHierarchy()
        {
            List<int> postIds = new List<int>();
            List<List<int>> listOfMerges = new List<List<int>>();

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


            for (int i = 0; i < postIds.Count; i++)
            {
                postInClusters.Add(postIds[i], new List<int>());

                List<int> idHit = listOfMerges.Where(pair => pair.Contains(i)).First();
                var clusterNumber = listOfMerges.IndexOf(idHit) + postIds.Count;
                postInClusters[postIds[i]].Add(clusterNumber);
                while (clusterNumber != (postIds.Count - 1) * 2)
                {
                    idHit = listOfMerges.Where(pair => pair.Contains(clusterNumber)).First();
                    clusterNumber = listOfMerges.IndexOf(idHit) + postIds.Count;
                    postInClusters[postIds[i]].Add(clusterNumber);
                }
            }
        }
    }
}
