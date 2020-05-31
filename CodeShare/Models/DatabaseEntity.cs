using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Models
{
    public abstract class DatabaseEntity : IEquatable<DatabaseEntity>
    {

        public string Id { get; set; } = Guid.NewGuid().ToString();

        public override bool Equals(object obj)
        {
            return Equals(obj as DatabaseEntity);
        }

        public bool Equals(DatabaseEntity other)
        {
            return other != null &&
                   Id == other.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public static bool operator ==(DatabaseEntity left, DatabaseEntity right)
        {
            return EqualityComparer<DatabaseEntity>.Default.Equals(left, right);
        }

        public static bool operator !=(DatabaseEntity left, DatabaseEntity right)
        {
            return !(left == right);
        }
    }
}
