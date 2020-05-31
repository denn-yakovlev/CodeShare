using CodeShare.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Services.DatabaseInteractor.MongoDB
{
    class MongoCollection<T> : IDataRepository<T> where T : DatabaseEntity
    {
        IMongoCollection<T> _collection;

        public MongoCollection(IMongoDatabase database, string collectionName)
        {
            _collection = database.GetCollection<T>(
                collectionName, new MongoCollectionSettings { AssignIdOnInsert = true }
                );
        }

        private static FilterDefinition<T> GetIdFilter(string id) =>
            Builders<T>.Filter.Eq("_id", id);

        private IFindFluent<T, T> FindById(string id) =>
            _collection.Find(GetIdFilter(id));

        private async Task<IFindFluent<T, T>> FindByIdAsync(string id) =>
            await Task.Run(() => FindById(id));

        public void Create(T item) => _collection.InsertOne(item);

        public async Task CreateAsync(T item) => await Task.Run(() => Create(item));

        public void Delete(string id) => 
            _collection.FindOneAndDelete(GetIdFilter(id));

        public async Task DeleteAsync(string id) => 
            await _collection.FindOneAndDeleteAsync(GetIdFilter(id));

        public T Read(string id) => FindById(id).Single();

        public IEnumerable<T> ReadAll() => _collection.Find(Builders<T>.Filter.Empty).ToList();

        public async Task<IEnumerable<T>> ReadAllAsync() =>
            await (await _collection.FindAsync(Builders<T>.Filter.Empty)).ToListAsync();

        public async Task<T> ReadAsync(string id) => (await FindByIdAsync(id)).Single();

        public void Update(string id, T updates)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(string id, T updates)
        {
            throw new NotImplementedException();
        }
    }
}
