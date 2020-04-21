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
        public ThreadService(IThreadRepository threadRepository)
        {
            this.threadRepository = threadRepository;
        }

        public string CreateNewThread(Thread newThread)
        {
            var id = ObjectId.GenerateNewId().ToString();
            newThread.Id = id;
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
            var mockPosts = new List<ThreadPost>();
            mockPosts.Add(
                new ThreadPost
                {
                    Id = "fakeId1",
                    ThreadIndex = 0,
                    Body = "That is the questions real description thi is a really long body",
                    Title = "What is the question?",
                    ConnectedPosts = new List<string> { "fakeId2" }
                });
            mockPosts.Add(new ThreadPost
            {
                Id = "fakeId2",
                ThreadIndex = 1,
                Body = "That is the questions real description thi is a really long body",
                Title = "What is the question 2?",
                ConnectedPosts = new List<string> { "fakeId1", "fakeId3" }
            });
            mockPosts.Add(new ThreadPost
            {
                Id = "fakeId3",
                ThreadIndex = 2,
                Body = "That is the questions real description thi is a really long body",
                Title = "What is the question 3?",
                ConnectedPosts = new List<string> { "fakeId2" }
            });

            if (!hasAccessToThread(id, askingUser))
            {
                throw new Exception("You have no access to this thread!");
            }

            return new SingleThread
            {
                Thread = threadRepository.Find(id),
                Posts = mockPosts.OrderBy(post => post.ThreadIndex).ToList()
            };
        }

        private bool hasAccessToThread(string threadId, string userId)
        {
            var thread = threadRepository.Find(threadId);

            return thread.Owner == userId;
        }
    }
}
