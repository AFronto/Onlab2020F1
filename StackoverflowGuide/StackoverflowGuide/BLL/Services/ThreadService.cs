using MongoDB.Bson;
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
        private IPostsBQRepository postsBQRepository;
        private IPostsRepository postsRepository;

        public ThreadService(IThreadRepository threadRepository, IPostsBQRepository postsBQRepository, IPostsRepository postsRepository)
        {
            this.threadRepository = threadRepository;
            this.postsBQRepository = postsBQRepository;
            this.postsRepository = postsRepository;
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

        public IEnumerable<Tag> GetAllTags()
        {
            var mockTags = new List<Tag>();
            mockTags.Add(
                new Tag
                {
                    Id = "fakeTagId1",
                    Name = "C",
                });
            mockTags.Add(
                new Tag
                {
                    Id = "fakeTagId2",
                    Name = "C++",
                });
            mockTags.Add(
                new Tag
                {
                    Id = "fakeTagId3",
                    Name = "C#",
                });
            mockTags.Add(
                new Tag
                {
                    Id = "fakeTagId4",
                    Name = "Java",
                });
            mockTags.Add(
                new Tag
                {
                    Id = "fakeTagId5",
                    Name = "Python",
                });

            return mockTags;

        }

        public SingleThread GetSingleThread(string id, string askingUser)
        {
            var thread = threadRepository.Find(id);
            var bqPosts = postsBQRepository.GetAllByIds(thread.ThreadPosts);
            var storedThreadPosts = postsRepository.Querry(p => thread.ThreadPosts.Contains(p.ThreadId));

            if (bqPosts.Count() != storedThreadPosts.Count())
            {
                throw new Exception("Cannot get the relevant posts!");
            }

            return new SingleThread
            {
                Thread = thread,
                Posts = bqPosts.Select(bqP =>
                {
                    var storedThreadPost = storedThreadPosts.Where(sTP => sTP.ThreadId == bqP.Id).First();
                    return new ThreadPost
                    {
                        Id = bqP.Id,
                        Title = bqP.Title,
                        Body = bqP.Body,
                        ThreadIndex = storedThreadPost.ThreadIndex,
                        ConnectedPosts = storedThreadPost.ConnectedPosts
                    };
                }).OrderBy(post => post.ThreadIndex).ToList()
            };
        }

        private bool hasAccessToThread(string threadId, string userId)
        {
            var thread = threadRepository.Find(threadId);

            return thread.Owner == userId;
        }
    }
}
