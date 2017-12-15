using Awesome.WorkFlowEngine.Interface;
using System;
using System.Linq;
using Awesome.WorkFlowEngine.Common;
using Awesome.WorkFlowEngine.WorkNode;
using Awesome.WorkFlowEngine.Base;
using Awesome.WorkFlowEngine.Core;
using System.Collections.Generic;
using Awesome.WorkFlowEngine.Events;

namespace Awesome.WorkFlowEngine
{

    public static partial class WorkFlowEngine
    {
        #region private
        private static IWorkItemBase Next(this IWorkItemBase context, IWorkSheetBase sheet)
        {
            var workNode = context.CurrentNode;
            var flow = workNode.Flow?.FirstOrDefault(p => p.From.Value == workNode.CurrentState.Value);
            var nextNode = flow?.To;
            if (nextNode != null && nextNode.Key != "end")
            {
                //节点流转
                nextNode = sheet.WorkFlow.Nodes.First(node => node.Id == nextNode.Id);
                context.CurrentNode = nextNode;
                //修改下一个节点的初始化状态
                context.CurrentNode.CurrentState = nextNode.BeginStatus;
                context.CurrentUser = DataProvider.GetCurrentUser(sheet, nextNode.Handler);
            }
            if (nextNode != null && nextNode.Key == "end")
            {
                context.CurrentNode = new WorkFlowNode
                {
                    Label = "工作流完成",
                    Key = "end",
                };
            }
            return context;
        }
        private static bool HasDataProvider
        {
            get
            {
                return DataProvider != null;
            }
        }
        internal static IDataProvider DataProvider
        {
            get;
            set;
        }

        private static void InvokeTransfers(IWorkItemBase item, IWorkSheetBase sheet)
        {
            TransferEventArgs args = new TransferEventArgs();
            //todo: check changes
            TransferEventPublisher.Invoke(args);
        }


        #endregion
        public static bool Start(IDataProvider dataProvider)
        {
            DataProvider = dataProvider ?? throw new Exception("no workflow engine dataProvider, system cant start engine");
            return HasDataProvider;
        }
        public static IWorkSheetBase Next(this IWorkSheetBase sheet)
        {
            if (!HasDataProvider)
            {
                throw new Exception("no workflow engine data provider, system cant set worksheet with nextnode");
            }
            foreach (var item in sheet.WorkItems)
            {
                item.Next(sheet);
                if (sheet.WorkItems.All(p => p.CurrentNode.Key == "end"))
                {
                    DataProvider.Final(sheet);
                }
                else
                {
                    DataProvider.LogInfo(sheet, $"工单{sheet.Id}的子项目{item.Id}当前处理节点流转到{item.CurrentNode.Name},流转到{item.CurrentUser}", "system", item.Id);
                    DataProvider.Commit(sheet);
                }

            }
            return sheet;
        }
        public static IWorkSheetBase Start<T>(this IWorkSheetBase sheet) where T : IWorkSheetBase
        {
            try
            {
                if (!HasDataProvider)
                {
                    throw new Exception("工作流引擎DataProvider为空, 无法启动");
                }
                var sheetInstance = (T)sheet;
                var workflow = sheetInstance.WorkFlow;
                foreach (var context in sheetInstance.WorkItems)
                {
                    var node = context.CurrentNode;
                    var startNode = workflow.Nodes?.First(p => p.Id == workflow.StartNodeId);
                    if (startNode.IsNull())
                    {
                        throw new Exception($"无法启动工单 #{sheet.Id} , 工单的工作流没有设置起点.");
                    }
                    else
                    {
                        context.CurrentNode = startNode;
                        context.CurrentNode.CurrentState = context.CurrentNode.BeginStatus;
                        context.CurrentUser = DataProvider.GetCurrentUser(sheet, context.CurrentNode.Handler);
                    }
                }
                DataProvider.Commit(sheet);
                DataProvider.LogInfo(sheet, $"工单 #{sheet.Id} 启动成功!", "system");
                return sheet;
            }
            catch (Exception ex)
            {
                DataProvider.LogInfo(sheet, $"工单 #{sheet.Id} 启动异常!{ex.Message}", "system");
                return sheet;
            }

        }
        public static string Log(this IWorkSheetBase sheet, string message, string currentUser, string workItemId = "")
        {
            if (!HasDataProvider)
            {
                throw new Exception("没有工作流引擎驱动, 系统暂无法记录您的日志:" + message);
            }
            else
            {
                return DataProvider.LogInfo(sheet, message, currentUser, workItemId);
            }
        }
    }
}