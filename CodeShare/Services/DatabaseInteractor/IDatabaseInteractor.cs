using CodeShare.Model.DTOs;

namespace CodeShare.Services.DatabaseInteractor
{
    public interface IDatabaseInteractor //: IDisposable
    {
        IDataRepository<User> Users { get; }

        IDataRepository<Project> Projects { get; }
    }
}
