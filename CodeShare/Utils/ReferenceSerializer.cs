using CodeShare.Model;
using CodeShare.Model.DTOs;
using CodeShare.Services.DatabaseInteractor;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace CodeShare.Utils
{
    public class ReferenceSerializer<T> : SerializerBase<Reference<T>> where T: DatabaseEntity
    {
        private IDataRepository<T> repo;

        public ReferenceSerializer(IDataRepository<T> repo)
        {
            this.repo = repo;
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Reference<T> value)
        {
            var writer = context.Writer;
            writer.WriteStartDocument();
            writer.WriteName("refId");
            writer.WriteString(value.RefId);
            writer.WriteEndDocument();
        }

        public override Reference<T> Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var reader = context.Reader;
            reader.ReadStartDocument();
            reader.ReadName(new Utf8NameDecoder());
            var refId = reader.ReadString();
            reader.ReadEndDocument();
            return new Reference<T>(refId, repo);
        }
    }
}
