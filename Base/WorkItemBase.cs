using Walle.WorkFlowEngine.Core;
using Walle.WorkFlowEngine.Interface;

namespace Walle.WorkFlowEngine.Base
{
    public class WorkItemBase : IWorkItemBase
    {
        public string Id { get; set; } = string.Empty;
        public WorkFlowNode CurrentNode { get; set; } = new WorkFlowNode();
        public string CurrentUser { get; set; } = string.Empty;
       
    }
}