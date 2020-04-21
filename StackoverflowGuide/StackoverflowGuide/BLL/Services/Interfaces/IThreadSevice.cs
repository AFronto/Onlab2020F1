using StackoverflowGuide.BLL.Models.Tag;
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

        public string DeleteThread(string id, string askingUser);

        public string EditThread(string id, Thread updatedThread);
        public IEnumerable<Thread> GetAll(string userId);
        public SingleThread GetSingleThread(string id, string askingUser);

        public IEnumerable<Tag> GetAllTags();
    }
}
