
using Awesome.WorkFlowEngine.Core;

namespace Awesome.WorkFlowEngine.Interface
{
    public interface IWorkItemBase
    {
        WorkFlowNode CurrentNode { get; set; }

        string Id { get; set; }

        string CurrentUser { get; set; }
    }
}
