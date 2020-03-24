using MongoDB.Bson;
using MongoDB.Driver;
using StackoverflowGuide.BLL.RepositoryInterfaces;
using StackoverflowGuide.DATA.Context;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StackoverflowGuide.DATA.Repositories
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly IMongoDBContext _context;
        protected readonly IMongoCollection<TEntity> DbSet;

        protected BaseRepository(IMongoDBContext context)
        {
            _context = context;
            DbSet = _context.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public virtual Task Add(TEntity obj)
        {
            return _context.AddCommand(async () => await DbSet.InsertOneAsync(obj));
        }

        public virtual async Task<TEntity> GetById(string id)
        {
            var data = await DbSet.FindAsync(Builders<TEntity>.Filter.Eq(" _id ", id));
            return data.FirstOrDefault();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            var all = await DbSet.FindAsync(Builders<TEntity>.Filter.Empty);
            return all.ToList();
        }

        public virtual Task Update(string id, TEntity obj)
        {
            return _context.AddCommand(async () =>
            {
                await DbSet.ReplaceOneAsync(Builders<TEntity>.Filter.Eq(" _id ", id), obj);
            });
        }

        public virtual Task Remove(string id) => _context.AddCommand(() => DbSet.DeleteOneAsync(Builders<TEntity>.Filter.Eq(" _id ", id)));

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
