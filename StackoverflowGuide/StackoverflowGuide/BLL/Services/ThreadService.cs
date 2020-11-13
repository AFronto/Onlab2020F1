using MongoDB.Bson;
using StackoverflowGuide.BLL.Helpers.Interfaces;
using StackoverflowGuide.BLL.Models.ElasticBLL;
using StackoverflowGuide.BLL.Models.Post;
using StackoverflowGuide.BLL.Models.Tag;
using StackoverflowGuide.BLL.Models.Thread;
using StackoverflowGuide.BLL.RepositoryInterfaces;
using StackoverflowGuide.BLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StackoverflowGuide.BLL.Services
{
    public class ThreadService : IThreadService
    {

        private IThreadRepository threadRepository;
        private IQuestionsElasticRepository questionsElasticRepository;
        private IPostsRepository postsRepository;
        private ITagRepository tagRepository;

        private IElasticSuggestionHelper elasticSuggestionHelper;

        public ThreadService(IThreadRepository threadRepository, IQuestionsElasticRepository questionsElasticRepository,
                                IPostsRepository postsRepository, ITagRepository tagRepository,
                                IElasticSuggestionHelper elasticSuggestionHelper)
        {
            this.threadRepository = threadRepository;
            this.questionsElasticRepository = questionsElasticRepository;
            this.postsRepository = postsRepository;
            this.tagRepository = tagRepository;

            this.elasticSuggestionHelper = elasticSuggestionHelper;
        }

        public string CreateNewThread(Thread newThread)
        {
            var id = ObjectId.GenerateNewId().ToString();
            newThread.Id = id;
            newThread.ThreadPosts = new List<string>();
            if (threadRepository.Create(newThread))
            {
                return id;
            }
            else
            {
                throw new Exception("Couldn't create a new thread!");
            }

        }

        public string DeleteThread(string id, string askingUser)
        {
            if (!hasAccessToThread(id, askingUser))
            {
                throw new Exception("You have no access to this thread!");
            }

            var thread = threadRepository.Find(id);
            foreach (var post in thread.ThreadPosts)
            {
                var postToDelete = postsRepository.Querry(p => p.Id == post).First();
                if (!(postsRepository.Delete(postToDelete.Id)))
                {
                    throw new Exception("Cannot delete nonexistant post!");
                }
            }

            if (threadRepository.Delete(id))
            {
                return id;
            }
            else
            {
                throw new Exception("Cannot delete nonexistant thread!");
            }
        }

        public string EditThread(string id, Thread updatedThread)
        {
            if (!hasAccessToThread(id, updatedThread.Owner))
            {
                throw new Exception("You have no access to this thread!");
            }

            var thread = threadRepository.Find(id);
            updatedThread.ThreadPosts = thread.ThreadPosts;

            if (threadRepository.Update(updatedThread))
            {
                return id;
            }
            else
            {
                throw new Exception("Cannot update nonexistant thread!");
            }
        }

        public IEnumerable<Thread> GetAll(string userId)
        {
            //TODO:finish and error handling
            return threadRepository.Querry(thread => thread.Owner == userId);
        }

        public IEnumerable<DbTag> GetAllTags()
        {
            return tagRepository.QuerryAll();
        }

        public SingleThread GetSingleThread(string id, string askingUser)
        {
            if (!hasAccessToThread(id, askingUser))
            {
                throw new Exception("You have no access to this thread!");
            }

            var thread = threadRepository.Find(id);
            var storedThreadPosts = postsRepository.Querry(p => thread.ThreadPosts.Contains(p.Id));
            var recommendedQuestions = elasticSuggestionHelper.GetRecommendedQuestions(storedThreadPosts.OrderByDescending(sTP => sTP.ThreadIndex)
                                                                                .Select(sTP => sTP.PostId)
                                                                                .ToList(),
                                                                "c# property",
                                                                thread.TagList.ToList());
            var threadQuestions = questionsElasticRepository.GetAllByIds(storedThreadPosts.Select(sTP => sTP.PostId)
                                                                                       .Concat(recommendedQuestions.Select(recQ => recQ.Id))
                                                                                       .ToList());


            if (threadQuestions.Count() != storedThreadPosts.Count() + recommendedQuestions.Count || threadQuestions.Count() == 0)
            {
                throw new Exception("Cannot get the relevant posts!");
            }

            return new SingleThread
            {
                Thread = thread,
                Posts = storedThreadPosts.Select(sTP =>
                {
                    var bqPost = threadQuestions.Where(tQ => sTP.PostId == tQ.Id).First();
                    return new ThreadPost
                    {
                        Id = sTP.Id,
                        Title = bqPost.Title,
                        Body = bqPost.Body,
                        ThreadIndex = sTP.ThreadIndex,
                        ConnectedPosts = sTP.ConnectedPosts
                    };
                }).OrderBy(post => post.ThreadIndex).ToList(),
                Suggestions = elasticSuggestionHelper.ParseQuestionsToThreadPosts(recommendedQuestions, storedThreadPosts.ToList())
            };
        }

    private bool hasAccessToThread(string threadId, string userId)
    {
        var thread = threadRepository.Find(threadId);

        return thread.Owner == userId;
    }
}
}
