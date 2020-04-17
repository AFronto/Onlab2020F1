using StackoverflowGuide.BLL.Models.Thread;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.Services.Interfaces
{
    public interface IThreadService
    {
        public string CreateNewThread(Thread newThread);

        public string DeleteThread(string id);

        public string EditThread(string id, Thread updatedThread);
        public IEnumerable<Thread> GetAll();
        public SingleThread GetSingleThread(string id);
    }
}
