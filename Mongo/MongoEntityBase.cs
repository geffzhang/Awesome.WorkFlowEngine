using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Walle.WorkFlowEngine.Mongo;

namespace Walle.WorkFlowEngine.Mongo
{
    public class MongoEntityBase : IMongoEntityBase
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonIgnore]
        public DateTime CreateTime
        {
            get
            {
                return DateTime.Parse(Id.CreationTime.ToLocalTime().ToString());
            }
        }
        /// <summary>
        ///  Id字符串
        /// </summary>
        /// <returns></returns>
        [BsonIgnore]
        public string _Id
        {
            get
            {
                return Id.ToString();
            }
        }
    }
}
