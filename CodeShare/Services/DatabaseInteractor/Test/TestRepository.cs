using CodeShare.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Services.DatabaseInteractor.Test
{
    public class TestRepository<T> : IDataRepository<T> where T : DatabaseEntity
    {
        private ConcurrentDictionary<string, T> repo = new ConcurrentDictionary<string, T>();

        public TestRepository(params T[] items)
        {
            foreach (var item in items)
            {
                repo.TryAdd(item.Id, item);
            }
        }

        public void Create(T item) => repo.TryAdd(item.Id, item);

        public async Task CreateAsync(T item) => await Task.Run(() => Create(item));

        public void Delete(string id) => repo.TryRemove(id, out _);

        public async Task DeleteAsync(string id) => await Task.Run(() => Delete(id));

        public IEnumerable<T> Filter(Predicate<T> filter)
        {
            return repo.Select(kvpair => kvpair.Value).Where(value => filter(value));
        }

        public Task<IEnumerable<T>> FilterAsync(Predicate<T> filter)
        {
            throw new NotImplementedException();
        }

        public T Read(string id) => repo[id];

        public IEnumerable<T> ReadAll()
        {
            foreach (var item in repo.Values)
            {
                yield return item;
            }
        }

        public async Task<IEnumerable<T>> ReadAllAsync() => await Task.Run(() => ReadAll());

        public async Task<T> ReadAsync(string id) => await Task.Run(() => Read(id));

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
