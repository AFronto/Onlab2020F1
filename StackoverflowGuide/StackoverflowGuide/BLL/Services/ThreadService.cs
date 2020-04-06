using MongoDB.Bson;
using StackoverflowGuide.BLL.Models.Thread;
using StackoverflowGuide.BLL.RepositoryInterfaces;
using StackoverflowGuide.BLL.Services.Interfaces;
using System;
using System.Collections.Generic;

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
            if(threadRepository.Create(newThread))
            {
                return id;
            }
            else
            {
                throw new Exception("Couldn't create a new thread!");
            }

        }

        public string DeleteThread(string id)
        {
            if(threadRepository.Delete(id))
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
            if (threadRepository.Update(updatedThread))
            {
                return id;
            }
            else
            {
                throw new Exception("Cannot update nonexistant thread!");
            }
        }

        public IEnumerable<Thread> GetAll()
        {
            //TODO:finish and error handling
            return threadRepository.QuerryAll();
        }
    }
}
