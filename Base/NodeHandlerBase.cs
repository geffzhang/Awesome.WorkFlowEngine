using Walle.WorkFlowEngine.Common;
using Walle.WorkFlowEngine.Core;
using Walle.WorkFlowEngine.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Walle.WorkFlowEngine.WorkNode.Attributes;
using System.Reflection;

namespace Walle.WorkFlowEngine.Base
{
    public class NodeHandlerBase<T> : INodeHandler where T : INodeHandler, new()
    {
        public IWorkItemBase WorkItem { get; set; }
        public IWorkSheetBase WorkSheet { get; set; }
        public INodeHandler CreateInstance(IWorkItemBase item, IWorkSheetBase sheet)
        {
            var result = new T();
            result.WorkItem = item;
            result.WorkSheet = sheet;
            return result;
        }

        public WorkFlowNodeStatus GetWorkFlowNodeStatus(Enum enumItem)
        {
            Type e = enumItem.GetType();
            FieldInfo field = e.GetField(enumItem.ToString());
            LabelAttribute[] laberAttrs = (LabelAttribute[])field.GetCustomAttributes(typeof(LabelAttribute), false);
            if (laberAttrs.Length > 0)
            {
                var result = new WorkFlowNodeStatus()
                {
                    Label = laberAttrs.First().Label,
                    Value = enumItem.ToString()
                };
                return result;
            }
            return new WorkFlowNodeStatus();
        }
    }
}
