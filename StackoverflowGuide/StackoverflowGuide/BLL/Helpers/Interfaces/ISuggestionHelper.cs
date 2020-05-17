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
        List<string> GetSuggestionIds();
        List<ThreadPost> ParseSuggestions(List<Post> bqPosts, List<StoredThreadPost> storedThreadPosts);
    }
}
