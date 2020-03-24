using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.DATA.Context
{
    public interface IMongoDBContext
    {
        public int SaveChanges();
        public IMongoCollection<T> GetCollection<T>(string name);
        public void Dispose();
        public Task AddCommand(Func<Task> func);

    }
}
