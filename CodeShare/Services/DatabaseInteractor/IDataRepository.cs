using CodeShare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Services.DatabaseInteractor
{
    public interface IDataRepository<TItem> where TItem : DatabaseEntity
    {
        void Create(TItem item);

        TItem Read(string id);

        IEnumerable<TItem> ReadAll();

        void Update(string id, TItem updates);

        void Delete(string id);

        Task CreateAsync(TItem item);

        Task<TItem> ReadAsync(string id);

        Task<IEnumerable<TItem>> ReadAllAsync();

        Task UpdateAsync(string id, TItem updates);

        Task DeleteAsync(string id);

        IEnumerable<TItem> Filter(Predicate<TItem> filter);

        Task<IEnumerable<TItem>> FilterAsync(Predicate<TItem> filter);
    }
}
