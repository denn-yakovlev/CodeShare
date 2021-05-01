using CodeShare.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Services.DatabaseInteractor.Test
{
    public class TestInteractor : IDatabaseInteractor
    {
        public IDataRepository<User> Users { get; } =  new TestRepository<User>(
            new User 
            {
                Id = "1",
                UserName = "admin",
                Password = "adminadmin"
            });

        public IDataRepository<Model.DTOs.Task> Tasks => throw new NotImplementedException();

        public IDataRepository<Session> Sessions { get; } = new TestRepository<Session>(
            new Session 
            { 
                Id = "1" 
            });
    }
}
