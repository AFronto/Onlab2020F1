using MongoDB.Bson;
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

        public string DeletePost(string threadId, string postId, string askingUser)
        {
            if (!hasAccessToThread(threadId, askingUser))
            {
                throw new Exception("You have no access to this thread!");
            }

            var thread = threadRepository.Find(threadId);
            thread.ThreadPosts.Remove(postId);
            threadRepository.Update(thread);

            var postToDelete = postsRepository.Querry(p => p.ThreadId == postId).First();
            var modifiedPosts = postsRepository.Querry(p => p.ConnectedPosts.Contains(postId));

            foreach(var post in modifiedPosts)
            {
                post.ConnectedPosts.Remove(postId);
                post.ConnectedPosts.AddRange(postToDelete.ConnectedPosts);
                postsRepository.Update(post);
            }

            var postsFollowing = postsRepository.Querry(p => p.ThreadIndex > postToDelete.ThreadIndex);
            foreach (var post in postsFollowing)
            {
                post.ThreadIndex -= 1;
                postsRepository.Update(post);
            }

            if (postsRepository.Delete(postToDelete.Id))
            {
                return threadId;
            }
            else
            {
                throw new Exception("Cannot delete nonexistant post!");
            }
        }

        public NewPostAndSuggestions GetSuggestionsAfterAccept(string threadId, ThreadPost acceptedPost, string askingUser)
        {
            if (!hasAccessToThread(threadId, askingUser))
            {
                throw new Exception("You have no access to this thread!");
            }


            var thread = threadRepository.Find(threadId);
            var dbId = ObjectId.GenerateNewId().ToString();
            thread.ThreadPosts.Add(dbId);
            threadRepository.Update(thread);

            var storedThreadPosts = postsRepository.Querry(p => thread.ThreadPosts.Contains(p.Id)).ToList();
            var acceptedStoredThreadPost = new StoredThreadPost
            {
                Id = dbId,
                ThreadId = acceptedPost.Id,
                ConnectedPosts = acceptedPost.ConnectedPosts,
                ThreadIndex = storedThreadPosts.Count() > 0 ? storedThreadPosts.MaxBy(sTP => sTP.ThreadIndex).First().ThreadIndex + 1 : 0
            };
            storedThreadPosts.Add(acceptedStoredThreadPost);
            postsRepository.Create(acceptedStoredThreadPost);

            var suggestion = suggestionHelper.GetSuggestionIds(storedThreadPosts.OrderByDescending(sTP => sTP.ThreadIndex)
                                                                                .Select(sTP => sTP.ThreadId)
                                                                                .ToList(),
                                                               thread.TagList.ToList());

            var bqPosts = postsBQRepository.GetAllByIds(suggestion);

            return new NewPostAndSuggestions
            {
                NewPost = new ThreadPost
                {
                    Id = acceptedStoredThreadPost.ThreadId,
                    ThreadIndex = acceptedStoredThreadPost.ThreadIndex,
                    Title = acceptedPost.Title,
                    Body = acceptedPost.Body,
                    ConnectedPosts = acceptedStoredThreadPost.ConnectedPosts
                },
                Suggestions = suggestionHelper.ParseSuggestions(bqPosts, storedThreadPosts)
            };
        }

        public List<ThreadPost> GetSuggestionsAfterDecline(string threadId, ThreadPost declinedPost, string askingUser)
        {
            if (!hasAccessToThread(threadId, askingUser))
            {
                throw new Exception("You have no access to this thread!");
            }


            var thread = threadRepository.Find(threadId);
            var storedThreadPosts = postsRepository.Querry(p => thread.ThreadPosts.Contains(p.Id));
            var suggestions = suggestionHelper.GetSuggestionIds(storedThreadPosts.OrderByDescending(sTP => sTP.ThreadIndex)
                                                                                .Select(sTP => sTP.ThreadId)
                                                                                .ToList(),
                                                                thread.TagList.ToList());
            var bqPosts = postsBQRepository.GetAllByIds(suggestions);


            return suggestionHelper.ParseSuggestions(bqPosts, storedThreadPosts.ToList());

        }

        private bool hasAccessToThread(string threadId, string userId)
        {
            var thread = threadRepository.Find(threadId);

            return thread.Owner == userId;
        }
    }
}
