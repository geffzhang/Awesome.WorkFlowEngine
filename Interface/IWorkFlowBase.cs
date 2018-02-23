using Walle.WorkFlowEngine.Core;
using System.Collections.Generic;

namespace Walle.WorkFlowEngine.Interface
{

    public interface IWorkFlowBase
    {
        string StartNodeId { get; set; }

        IEnumerable<WorkFlowNode> Nodes { get; set; }
    }
}
