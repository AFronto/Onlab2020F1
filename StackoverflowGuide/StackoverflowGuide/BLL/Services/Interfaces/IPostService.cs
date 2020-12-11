using StackoverflowGuide.BLL.Models.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.Services.Interfaces
{
    public interface IPostService
    {
        public List<ThreadPost> GetSuggestionsAfterDecline(string threadId, ThreadPost declinedPost, string askingUser);

        public NewPostAndSuggestions GetSuggestionsAfterAccept(string threadId, ThreadPost acceptedPost, string askingUser);

        public string DeletePost(string threadId, string postId, string askingUser);

        public SinglePost GetSingleById(string questionId);
    }
}
