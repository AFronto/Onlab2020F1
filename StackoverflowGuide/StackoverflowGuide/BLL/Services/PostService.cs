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
        IQuestionsElasticRepository questionsElasticRepository;
        IThreadRepository threadRepository;
        IPostsRepository postsRepository;
        IBQSuggestionHelper suggestionHelper;
        IElasticSuggestionHelper elasticSuggestionHelper;

        public PostService(IQuestionsElasticRepository questionsElasticRepository, IThreadRepository threadRepository,
                           IPostsRepository postsRepository, IBQSuggestionHelper suggestionHelper, IElasticSuggestionHelper elasticSuggestionHelper)
        {
            this.questionsElasticRepository = questionsElasticRepository;
            this.threadRepository = threadRepository;
            this.postsRepository = postsRepository;
            this.suggestionHelper = suggestionHelper;
            this.elasticSuggestionHelper = elasticSuggestionHelper;
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

            var postToDelete = postsRepository.Querry(p => p.Id == postId).First();
            var modifiedPosts = postsRepository.Querry(p => p.ConnectedPosts.Contains(postId));

            foreach(var post in modifiedPosts)
            {
                post.ConnectedPosts.Remove(postId);
                post.ConnectedPosts.AddRange(postToDelete.ConnectedPosts);
                postsRepository.Update(post);
            }

            var postsFollowing = postsRepository.Querry(p => p.ThreadIndex > postToDelete.ThreadIndex && p.ThreadId == threadId);
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
                ThreadId = threadId,
                PostId = acceptedPost.Id,
                ConnectedPosts = acceptedPost.ConnectedPosts,
                ThreadIndex = storedThreadPosts.Count() > 0 ? storedThreadPosts.MaxBy(sTP => sTP.ThreadIndex).First().ThreadIndex + 1 : 0
            };
            storedThreadPosts.Add(acceptedStoredThreadPost);
            postsRepository.Create(acceptedStoredThreadPost);

            var recommendedQuestions = elasticSuggestionHelper.GetRecommendedQuestions(storedThreadPosts.OrderByDescending(sTP => sTP.ThreadIndex)
                                                                                .Select(sTP => sTP.PostId)
                                                                                .ToList(),
                                                                thread.LastSearched,
                                                                thread.TagList.ToList());
            var threadQuestions = questionsElasticRepository.GetAllByIds(storedThreadPosts.Select(sTP => sTP.PostId)
                                                                                       .Concat(recommendedQuestions.Select(recQ => recQ.Id))
                                                                                       .ToList());

            return new NewPostAndSuggestions
            {
                NewPost = new ThreadPost
                {
                    Id = acceptedStoredThreadPost.Id,
                    ThreadIndex = acceptedStoredThreadPost.ThreadIndex,
                    Title = acceptedPost.Title,
                    Body = acceptedPost.Body,
                    ConnectedPosts = acceptedStoredThreadPost.ConnectedPosts
                },
                Suggestions = elasticSuggestionHelper.ParseQuestionsToThreadPosts(recommendedQuestions, storedThreadPosts.ToList())
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
            var recommendedQuestions = elasticSuggestionHelper.GetRecommendedQuestions(storedThreadPosts.OrderByDescending(sTP => sTP.ThreadIndex)
                                                                                .Select(sTP => sTP.PostId)
                                                                                .ToList(),
                                                                thread.LastSearched,
                                                                thread.TagList.ToList());
            var threadQuestions = questionsElasticRepository.GetAllByIds(storedThreadPosts.Select(sTP => sTP.PostId)
                                                                                       .Concat(recommendedQuestions.Select(recQ => recQ.Id))
                                                                                       .ToList());


            return elasticSuggestionHelper.ParseQuestionsToThreadPosts(recommendedQuestions, storedThreadPosts.ToList());

        }

        private bool hasAccessToThread(string threadId, string userId)
        {
            var thread = threadRepository.Find(threadId);

            return thread.Owner == userId;
        }
    }
}
