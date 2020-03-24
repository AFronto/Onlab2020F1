using StackoverflowGuide.BLL.Models.Thread;
using StackoverflowGuide.BLL.RepositoryInterfaces;
using StackoverflowGuide.BLL.Services.Interfaces;
using System;

namespace StackoverflowGuide.BLL.Services
{
    public class ThreadService : IThreadService
    {

        private IThreadRepository threadRepository;
        public ThreadService(IThreadRepository threadRepository)
        {
            this.threadRepository = threadRepository;
        }
        public Guid CreateNewThread(Thread newThread)
        {
            var id = Guid.NewGuid();
            newThread.Id = id.ToString();
            threadRepository.Add(newThread);
            var rep = threadRepository.GetAll();
            return id;
        }
    }
}
