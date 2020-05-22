using StackoverflowGuide.API.DTOs.Thread;
using StackoverflowGuide.BLL.Models.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.Helpers.Interfaces
{
    public interface ISuggestionHelper
    {
        public List<string> GetSuggestionIds(List<string> incomingIds, List<string> tagsFromThread);
        public List<ThreadPost> ParseSuggestions(List<Post> bqPosts, List<StoredThreadPost> storedThreadPosts);
        public List<int> GetPostsFromCluster(int clusterId);
    }
}
