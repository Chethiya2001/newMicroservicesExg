
using System.Linq.Expressions;
using MongoDB.Driver;
using Play.Common;


namespace Play.MongoDB.Common
{
    public class MongoRepository<T> : IRepository<T> where T : IEntity
    {

        private readonly IMongoCollection<T> dbCollection;
        private readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;

        public MongoRepository(IMongoDatabase database, string collectionName)
        {

            dbCollection = database.GetCollection<T>(collectionName);


        }

        //APIs
        public async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }
        public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await dbCollection.Find(filter).ToListAsync();
        }
        public async Task<T> GetItems(Guid id)
        {
            FilterDefinition<T> filter = filterBuilder.Eq(entity => entity.Id, id);
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }
        public async Task<T> GetItems(Expression<Func<T, bool>> filter)
        {
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }
        public async Task CreateAsync(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            await dbCollection.InsertOneAsync
                (item);
        }
        public async Task UpdateAsync(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            FilterDefinition<T> filter = filterBuilder.Eq(entity => entity.Id, item.Id);

            await dbCollection.ReplaceOneAsync(filter, item);
        }
        public async Task RemoveAync(Guid id)
        {
            FilterDefinition<T> filter = filterBuilder.Eq(entity => entity.Id, id);
            await dbCollection.DeleteOneAsync(filter);
        }




    }
}
