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
        public IEnumerable<Thread> GetAll();
    }
}
