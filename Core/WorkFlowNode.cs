using System.Collections.Generic;

namespace Awesome.WorkFlowEngine.Core
{
    public class WorkFlowNode
    {
  
        /// <summary>
        /// 内置节点id
        /// </summary>
        /// <returns></returns>
        public string Id { get; set; }

        /// <summary>
        /// 内置节点类型
        /// </summary>
        /// <returns></returns>
        public string Key { get; set; }

        /// <summary>
        /// 内置节点名称
        /// </summary>
        /// <returns></returns>
        public string Name { get; set; }

        /// <summary>
        /// 内置节点名称
        /// </summary>
        /// <returns></returns>
        public string Label { get; set; }

        /// <summary>
        /// 节点当前状态
        /// </summary>
        /// <returns></returns>
        public WorkFlowNodeStatus CurrentState { get; set; } = new WorkFlowNodeStatus();

        /// <summary>
        /// 节点流转设置
        /// </summary>
        /// <returns></returns>
        public List<WorkFlowSettings> Flow { get; set; } = new List<WorkFlowSettings>();

        /// <summary>
        /// 节点内部设置
        /// </summary>
        /// <returns></returns>
        public List<WorkFlowNodeSettings> Config { get; set; } = new List<WorkFlowNodeSettings>();

        /// <summary>
        /// 节点状态集
        /// </summary>
        /// <returns></returns>
        public List<WorkFlowNodeStatus> Status { get; set; } = new List<WorkFlowNodeStatus>();

        /// <summary>
        /// 开始状态
        /// </summary>
        public WorkFlowNodeStatus BeginStatus { get; set; } = new WorkFlowNodeStatus();

        /// <summary>
        /// 结束状态
        /// </summary>
        public WorkFlowNodeStatus EndStatus { get; set; } = new WorkFlowNodeStatus();

        /// <summary>
        /// 节点处理人
        /// </summary>
        /// <returns></returns>
        public WorkFlowActorUser Handler { get; set; } = new WorkFlowActorUser();
    }
}