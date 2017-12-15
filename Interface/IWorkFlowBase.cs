using Awesome.WorkFlowEngine.Core;
using System.Collections.Generic;

namespace Awesome.WorkFlowEngine.Interface
{

    public interface IWorkFlowBase
    {
        string StartNodeId { get; set; }

        IEnumerable<WorkFlowNode> Nodes { get; set; }
    }
}
