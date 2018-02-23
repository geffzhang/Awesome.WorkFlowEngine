using Walle.WorkFlowEngine.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Walle.WorkFlowEngine.Events
{
    public class TransferEventArgs : EventArgs
    {
        public IWorkSheetBase WorkSheet { get; set; } = null;
        public IWorkItemBase WorkItem { get; set; } = null;
        public List<TransferedDetail> Transfered = new List<TransferedDetail>();
    }

    public enum TransferedType
    {
        None,
        CurrentUser,
        CurrentNode,
        CurrentStatus,
    }

    public class TransferedDetail
    {
        public TransferedType Type = TransferedType.None;
        public Object OldValue = null;
        public Object NewValue = null;
    }

    public class TransferedDetail<T> 
    {
        public TransferedType Type = TransferedType.None;
        public T OldValue = default(T);
        public T NewValue = default(T);
    }
}
