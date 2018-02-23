namespace Walle.WorkFlowEngine.Core
{
    public class WorkFlowSettings
    {
        /// <summary>
        /// 节点状态
        /// </summary>
        /// <returns></returns>
        public WorkFlowNodeStatus From { get; set; }

        /// <summary>
        ///  流转目标节点
        /// </summary>
        /// <returns></returns>
        public WorkFlowNode To { get; set; }
    }
}