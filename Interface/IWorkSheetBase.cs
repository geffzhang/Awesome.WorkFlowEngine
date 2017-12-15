using Awesome.WorkFlowEngine.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Awesome.WorkFlowEngine.Interface
{
    public interface IWorkSheetBase
    {
        [BsonId]
        ObjectId Id
        {
            get; set;
        }

        [BsonIgnore]
        WorkFlowBase WorkFlow
        {
            get;
            set;
        }

        [BsonIgnore]
        IEnumerable<WorkItemBase> WorkItems { get; set; }


        List<string> GetValue(string key);
    }
}
