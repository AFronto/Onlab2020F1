using MongoDB.Driver;
using StackoverflowGuide.BLL.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.DATA.Context
{
    public interface IMongoDBContext
    {
        public IMongoCollection<T> GetCollection<T>() where T : DBModel;

    }
}
