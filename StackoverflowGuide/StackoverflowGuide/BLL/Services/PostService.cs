using MoreLinq;
using StackoverflowGuide.BLL.Helpers.Interfaces;
using StackoverflowGuide.BLL.Models.Post;
using StackoverflowGuide.BLL.RepositoryInterfaces;
using StackoverflowGuide.BLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.Services
{
    public class PostService : IPostService
    {
        IPostsBQRepository postsBQRepository;
        IThreadRepository threadRepository;
        IPostsRepository postsRepository;
        ISuggestionHelper suggestionHelper;

        public PostService(IPostsBQRepository postsBQRepository, IThreadRepository threadRepository,
                           IPostsRepository postsRepository, ISuggestionHelper suggestionHelper)
        {
            this.postsBQRepository = postsBQRepository;
            this.threadRepository = threadRepository;
            this.postsRepository = postsRepository;
            this.suggestionHelper = suggestionHelper;
        }

        public List<ThreadPost> GetSuggestions(string threadId)
        {
            var mockSugestions = suggestionHelper.GetSuggestionIds();

            var thread = threadRepository.Find(threadId);
            var bqPosts = postsBQRepository.GetAllByIds(mockSugestions);
            var storedThreadPosts = postsRepository.Querry(p => thread.ThreadPosts.Contains(p.ThreadId));

            return suggestionHelper.ParseSuggestions(bqPosts, storedThreadPosts.ToList());

        }
    }
}
