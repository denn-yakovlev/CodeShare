using CodeShare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Services.DatabaseInteractor
{
    public interface IDatabaseInteractor //: IDisposable
    {
        IDataRepository<User> Users { get; }

        IDataRepository<Project> Projects { get; }
    }
}
