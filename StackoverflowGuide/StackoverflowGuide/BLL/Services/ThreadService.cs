using MongoDB.Bson;
using StackoverflowGuide.BLL.Helpers.Interfaces;
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
        private ITagBQRepository tagBQRepository;
        private ITagRepository tagRepository;
        private IBQSuggestionHelper suggestionHelper;

        public ThreadService(IThreadRepository threadRepository, IQuestionsElasticRepository questionsElasticRepository,
                                IPostsRepository postsRepository, IBQSuggestionHelper suggestionHelper,
                                ITagBQRepository tagBQRepository, ITagRepository tagRepository)
        {
            this.threadRepository = threadRepository;
            this.questionsElasticRepository = questionsElasticRepository;
            this.postsRepository = postsRepository;
            this.suggestionHelper = suggestionHelper;
            this.tagBQRepository = tagBQRepository;
            this.tagRepository = tagRepository;
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
            var suggestions = suggestionHelper.GetSuggestionIds(storedThreadPosts.OrderByDescending(sTP => sTP.ThreadIndex)
                                                                                .Select(sTP => sTP.PostId)
                                                                                .ToList(),
                                                                thread.TagList.ToList());
            var bqPosts = questionsElasticRepository.GetAllByIds(storedThreadPosts.Select(sTP => sTP.PostId).Concat(suggestions).ToList())
                          .Select(q => new Post() { Id = q.Id, Body = q.Body, Title = q.Title }).ToList();

            // TEST
            questionsElasticRepository.SearchByText("inheritance", new List<string>() { "Title"});
            // TEST

            if (bqPosts.Count() != storedThreadPosts.Count() + suggestions.Count)
            {
                throw new Exception("Cannot get the relevant posts!");
            }

            return new SingleThread
            {
                Thread = thread,
                Posts = storedThreadPosts.Select(sTP =>
                {
                    var bqPost = bqPosts.Where(bqP => sTP.PostId == bqP.Id).First();
                    return new ThreadPost
                    {
                        Id = sTP.Id,
                        Title = bqPost.Title,
                        Body = bqPost.Body,
                        ThreadIndex = sTP.ThreadIndex,
                        ConnectedPosts = sTP.ConnectedPosts
                    };
                }).OrderBy(post => post.ThreadIndex).ToList(),
                Suggestions = suggestionHelper.ParseSuggestions(bqPosts
                                                                .Where(bqP => suggestions.Contains(bqP.Id))
                                                                .ToList(),
                                                               storedThreadPosts.ToList())
            };
        }

        private bool hasAccessToThread(string threadId, string userId)
        {
            var thread = threadRepository.Find(threadId);

            return thread.Owner == userId;
        }
    }
}
