using CodeShare.Model.DTOs;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Services.DatabaseInteractor.MongoDB
{
    public class MongoInteractor : IDatabaseInteractor
    {
        private MongoClient _mongoClient;

        private IMongoDatabase _database;

        public MongoInteractor(string connectionString, string databaseName)
        {
            _mongoClient = new MongoClient(connectionString);
            _database = _mongoClient.GetDatabase(databaseName);
            Users = new MongoRepository<User>(_database, "users");
            Tasks = new MongoRepository<Model.DTOs.Task>(_database, "projects");
            Sessions = new MongoRepository<Session>(_database, "sessions");
        }

        public IDataRepository<User> Users { get; } 

        public IDataRepository<Model.DTOs.Task> Tasks { get; }

        public IDataRepository<Session> Sessions { get; }

        //public void Dispose()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
