using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Awesome.WorkFlowEngine.Mongo
{
    public interface IMongoEntityBase
	{
        [BsonId]
		ObjectId Id { get; set; }
	}
}
