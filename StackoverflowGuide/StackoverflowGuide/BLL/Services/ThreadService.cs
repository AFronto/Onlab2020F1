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
            //TODO:finish and error handling
            var id = ObjectId.GenerateNewId().ToString();
            newThread.Id = id;
            threadRepository.Create(newThread);
            return id;
        }

        public string DeleteThread(string id)
        {
            if(threadRepository.Find(id) != null)
            {
                threadRepository.Delete(id);
                return id;
            }
            else
            {
                throw new Exception("Cannot delete nonexistant thread!");
            }
        }

        public IEnumerable<Thread> GetAll()
        {
            //TODO:finish and error handling
            return threadRepository.QuerryAll();
        }
    }
}
