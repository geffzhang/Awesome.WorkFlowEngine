using Walle.WorkFlowEngine.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Walle.WorkFlowEngine.Interface
{
    public interface INodeHandler
    {
        IWorkItemBase WorkItem { get; set; }
        IWorkSheetBase WorkSheet { get; set; }
        INodeHandler CreateInstance(IWorkItemBase item, IWorkSheetBase sheet);
    }
}
