using Walle.WorkFlowEngine.Core;
using System.Collections.Generic;
using System.Linq;
using Walle.WorkFlowEngine.Interface;
using System;

using Walle.WorkFlowEngine.Common;
using Walle.WorkFlowEngine.WorkNode;
using Walle.WorkFlowEngine.Base;
using System.Reflection;
using Walle.WorkFlowEngine.WorkNode.Attributes;

namespace Walle.WorkFlowEngine
{
    public static partial class WorkFlowEngine
    {
        public static WorkFlowNode QueryNextNode(this WorkFlowNode node)
        {
            var nextNode = node.Flow.First(flow => flow.From.Value == node.EndStatus.Value)?.To;
            return nextNode;
        }

        public static List<WorkFlowNode> QueryRelatedNodes(this WorkFlowNode node)
        {
            var flows = node.Flow;
            var result = flows.Select(p => p.To);
            return result?.ToList();
        }

        public static class Container
        {
            public static bool LoadHandler<T>() where T : INodeHandler, new()
            {
                return WorkNode.NodeHandlerContainer.Load<T>();
            }

            public static List<WorkFlowNode> Nodes
            {
                get
                {
                    return WorkNode.NodeContainer.NodeList;
                }
            }

            public static List<WorkFlowNodeRequire> RequireDetail
            {
                get
                {
                    return WorkNode.NodeHandlerContainer.requires;
                }
            }

            public static IEnumerable<WorkFlowNodeMethod> GetMethods(WorkFlowNode node)
            {
                var methods = WorkNode.NodeHandlerContainer.GetHandlerMethods(node.Key, node.CurrentState.Value);
                return methods;
            }
        }
    }

    public static class NodeHandler
    {
        public static bool CheckRequire(IWorkSheetBase sheet, IWorkItemBase item)
        {
            try
            {
                var require = WorkFlowEngine.Container.RequireDetail.Find(p => p.Key == item.CurrentNode.Key);
                if (require.IsNotNull())
                {
                    var result = WorkFlowEngine.DataProvider.CheckRequire(sheet, item, require);
                    return result;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                WorkFlowEngine.DataProvider.LogError($"CheckRequire异常", ex);
                return true;
            }


        }

        public static object Excete(this WorkFlowNodeMethod method, IWorkSheetBase sheet, IWorkItemBase item, out string message)
        {
            message = default(String);
            if (item.CurrentNode.Key != method.Key)
            {
                message = "子工单当前状态不适用此处理方法";
                return null;
            }
            try
            {
                return NodeHandlerContainer.Excete(method, sheet, item, method.Parameters.Select(p => p.Value).ToArray());
            }
            catch (System.Exception ex)
            {
                message = ex.Message;
                throw ex;
            }
        }
    }

    public static class Transfer
    {
        public static TItem To<TItem>(this IWorkItemBase item) where TItem : IWorkItemBase
        {
            var result = (TItem)item;
            return result;
        }
        public static TSheet To<TSheet>(this IWorkSheetBase sheet) where TSheet : IWorkSheetBase
        {
            var result = (TSheet)sheet;
            return result;
        }

        public static WorkFlowNodeStatus ToNodeState(this Enum enumItem)
        {
            try
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
                throw new Exception("无法将此枚举项转化为工作流节点状态项,请参考节点处理器中的节点状态设置代码要求.");
            }
            catch (Exception ex)
            {
                throw new Exception("无法将此枚举项转化为工作流节点状态项,发现异常.", ex);
            }
        }
        public static WorkFlowNodeSettings GetConfig(this List<WorkFlowNodeSettings> configs, Enum configEnum)
        {
            return configs.Where(cfg => cfg.Key == configEnum.ToString())?.First();
        }
    }



}
