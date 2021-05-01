using CodeShare.Model.DTOs;
using CodeShare.Services.DatabaseInteractor;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace CodeShare.Model
{
    public class Reference<T> where T : DatabaseEntity
    {
        [BsonElement("RefId")]
        public string RefId { get; }

        private IDataRepository<T> repo;

        public Reference(string refId, IDataRepository<T> repo)
        {
            RefId = refId;
            this.repo = repo;
        }

        public T? ReferencedItem
        {
            get
            {
                T? item;
                try
                {
                    item = repo.Read(RefId);
                }
                catch (MongoException)
                {
                    item = null;
                }
                return item;
            }
        }

        public static implicit operator T? (Reference<T> @ref) => @ref.ReferencedItem;
    }
}
