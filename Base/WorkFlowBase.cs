using Awesome.WorkFlowEngine.Core;
using Awesome.WorkFlowEngine.Interface;
using System.Collections.Generic;
using Awesome.WorkFlowEngine.Mongo;

namespace Awesome.WorkFlowEngine.Base
{
    /// <summary>
    /// 工作流对象
    /// </summary>
    public class WorkFlowBase : MongoEntityBase, IWorkFlowBase
    {
        /// <summary>
        /// 工作流名称
        /// </summary>
        /// <returns></returns>   
        public string Name { get; set; } = string.Empty;
 
        /// <summary>
        /// 标记起始节点位置
        /// </summary>
        public string StartNodeId { get; set; } = string.Empty;

        public IEnumerable<WorkFlowNode> Nodes { get; set; } = new List<WorkFlowNode>();

    }
}