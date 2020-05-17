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
        public SugesstionHelper()
        {

        }
        public List<string> GetSuggestionIds()
        {
            return new List<string> { "13", "19", "24" };
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
    }
}
