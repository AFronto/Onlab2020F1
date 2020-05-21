using MongoDB.Bson;
using MongoDB.Driver;
using StackoverflowGuide.BLL.Models.DB;
using StackoverflowGuide.BLL.RepositoryInterfaces;
using StackoverflowGuide.DATA.Context;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StackoverflowGuide.DATA.Repositories
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity:DBModel
    {
        private IMongoDBContext _dbContext;
        public IMongoCollection<TEntity> Collection { get; private set; }
        public BaseRepository(IMongoDBContext dbContext)
        {
            this._dbContext = dbContext;
            Collection = _dbContext.GetCollection<TEntity>();
        }

        public TEntity Find(string id)
        {
            ObjectId objectId;
            if (!ObjectId.TryParse(id.ToString(), out objectId))
            {
                return null;
            }
            var filterId = Builders<TEntity>.Filter.Eq("_id", objectId);
            var model = Collection.Find(filterId).FirstOrDefault();
            return model;
        }

        public bool Update(TEntity model)
        {
            ObjectId objectId;
            if (!ObjectId.TryParse(model.Id.ToString(), out objectId))
            {
                return false;
            }
            var filterId = Builders<TEntity>.Filter.Eq("_id", objectId);
            var updated = Collection.FindOneAndReplace(filterId, model);
            return updated != null;
        }

        public bool Create(TEntity model)
        {
            try
            {
                Collection.InsertOne(model);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool Delete(string id)
        {
            ObjectId objectId;
            if (!ObjectId.TryParse(id.ToString(), out objectId))
            {
                return false;
            }
            var filterId = Builders<TEntity>.Filter.Eq("_id", objectId);
            var deleted = Collection.FindOneAndDelete(filterId);
            return deleted != null;
        }

        public IEnumerable<TEntity> QuerryAll()
        {
            return Collection.Find(FilterDefinition<TEntity>.Empty).ToList();
        }
        public IEnumerable<TEntity> Querry(Expression<Func<TEntity, bool>> filter)
        {
            return Collection.Find(filter).ToList();
        }

        public DeleteResult EmptyTable()
        {
            return Collection.DeleteMany(FilterDefinition<TEntity>.Empty);
        }
    }
}
