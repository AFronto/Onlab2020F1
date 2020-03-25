using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using StackoverflowGuide.BLL.Models.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace StackoverflowGuide.DATA.Context
{
    public class MongoDBContext : IMongoDBContext
    {
        public MongoClient MongoDBClient { get; set; }
        public IMongoDatabase Database { get; set; }
            
        public MongoDBContext(IConfiguration configuration)
        {
            MongoDBClient = new MongoClient(configuration.GetSection("MongoSettings").GetSection("Connection").Value);

            Database = MongoDBClient.GetDatabase(configuration.GetSection("MongoSettings").GetSection("DatabaseName").Value);
        }

        public IMongoCollection<T> GetCollection<T>() where T: DBModel
        {
            var table = typeof(T).GetCustomAttribute<TableAttribute>(false).Name;
            return Database.GetCollection<T>(table);
        }
    }
}
