using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using IdentityService.Domain;
using MongoDB.Bson;
using MongoDB.Driver;

namespace IdentityService.Repository
{
    public class Repository<TPrimary, T> : IRepository<TPrimary, T>
        where TPrimary : class
        where T : class, IEntity, new()
    {
        protected readonly IMongoDbContext MongoDbContext;

        #region Fields
        private bool isDisposed;
        #endregion

        #region Constructor
        public Repository(IMongoDbContext mongoDbContext)
        {
            this.MongoDbContext = mongoDbContext;
        }
        #endregion

        #region Deconstructor
        ~Repository()
        {
            Dispose(false);
        }
        #endregion

        #region Protected Methods
        protected virtual IQueryable<T> LoadSync()
        {
            return MongoDbContext.Database.GetCollection<T>(typeof(T).Name).AsQueryable();
        }
        #endregion

        #region Query Methods
        public virtual async Task<T> Get(TPrimary id)
        {
            var cursor = await MongoDbContext.Database.GetCollection<T>(typeof(T).Name).FindAsync(GetIdFilter(id));
            var result = await cursor.SingleOrDefaultAsync();
            return result;
        }

        public virtual Task<T> Get(Expression<Func<T, bool>> predicate)
        {
            return Task.Factory.StartNew(() => MongoDbContext.Database.GetCollection<T>(typeof(T).Name).AsQueryable().SingleOrDefault(predicate.Compile()));
        }

        public virtual Task<IQueryable<T>> Load(Expression<Func<T, bool>> predicate)
        {
            return Task.Factory.StartNew(() => LoadSync().Where(predicate.Compile()).AsQueryable());
        }

        public virtual Task Delete(TPrimary id)
        {
            return Task.Factory.StartNew(async () =>
            {
                MongoDbContext.Database.GetCollection<T>(typeof(T).Name).DeleteOneAsync(GetIdFilter(id)).Wait();
            });
        }

        public virtual Task<T> Update(T entity)
        {
            return Task.Factory.StartNew(() =>
            {
                MongoDbContext.Database.GetCollection<T>(typeof(T).Name).ReplaceOneAsync(GetIdFilter(entity.Id), entity).Wait();
                return entity;
            });
        }

        public virtual Task<T> Save(T entity)
        {
            return Task.Factory.StartNew(() =>
            {
                MongoDbContext.Database.GetCollection<T>(typeof(T).Name).InsertOneAsync(entity).GetAwaiter().GetResult();
                return entity;
            });
        }
        #endregion

        #region Private Methods
        private FilterDefinition<T> GetDeletedIdFilter(TPrimary id)
        {
            var builder = Builders<T>.Filter;
            var filter = builder.Eq(entity => entity.Id, ObjectId.Parse(id.ToString()));
            
            return filter;
        }

        private FilterDefinition<T> GetIdFilter(TPrimary id)
        {
            var builder = Builders<T>.Filter;
            var filter = builder.Eq(entity => entity.Id, ObjectId.Parse(id.ToString()));

            return filter;
        }

        private FilterDefinition<T> GetIdFilter(ObjectId id)
        {
            var builder = Builders<T>.Filter;
            var filter = builder.Eq(entity => entity.Id, id);
            
            return filter;
        }
        #endregion

        #region IDisposable Methods
        public void Dispose()
        {
            if (!isDisposed)
                Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            isDisposed = true;
            if (disposing)
            {
                // Libera os componentes
            }
        }
        #endregion
    }
}