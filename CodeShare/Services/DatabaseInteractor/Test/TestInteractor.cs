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
                Id = "12345",
                UserName = "admin",
                Password = "adminadmin"
            });

        public IDataRepository<Model.DTOs.Task> Tasks => new TestRepository<Model.DTOs.Task>(
            new Model.DTOs.Task
            {
                Id = "123",
                Name = "Some task",
                Description = "Some description"
            });

        public IDataRepository<Session> Sessions { get; } = new TestRepository<Session>(
            new Session 
            { 
                Id = "1" 
            });
    }
}
