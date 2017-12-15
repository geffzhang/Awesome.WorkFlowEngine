using Awesome.WorkFlowEngine.Core;
using Awesome.WorkFlowEngine.Interface;

namespace Awesome.WorkFlowEngine.Base
{
    public class WorkItemBase : IWorkItemBase
    {
        public string Id { get; set; } = string.Empty;
        public WorkFlowNode CurrentNode { get; set; } = new WorkFlowNode();
        public string CurrentUser { get; set; } = string.Empty;
       
    }
}