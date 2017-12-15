using Awesome.WorkFlowEngine.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Context;
using Awesome.WorkFlowEngine.WorkNode.Attributes;
using Awesome.WorkFlowEngine.Common;
using Awesome.WorkFlowEngine.Core;
using System.Linq;

namespace Awesome.WorkFlowEngine.WorkNode
{
    internal static class NodeHandlerContainer
    {
        internal static Dictionary<string, INodeHandler> handlers = new Dictionary<string, INodeHandler>();
        internal static List<WorkFlowNodeRequire> requires = new List<WorkFlowNodeRequire>();
        internal static bool Load<T>() where T : INodeHandler, new()
        {
            try
            {
                var key = NodeContainer.Load<T>();
                var instance = new T();
                handlers.Add(key, instance);
                var require = new WorkFlowNodeRequire().Load(instance);
                if (require.IsNotNull())
                {  
                    requires.Add(require);
                }
                return true;
            }
            catch (Exception ex)
            {
                WorkFlowEngine.DataProvider.LogError($"工作流节点加载异常,节点:{typeof(T)}", ex);
                return false;
            }
        }
        private static IEnumerable<(MethodInfo Info, string Label)> GetMethods(string key, string state)
        {
            var item = handlers.FirstOrDefault(p => p.Key == key);
            if (item.IsNotNull())
            {
                var handler = item.Value;
                var type = handler.GetType();
                var methods = type.GetMethods();
                if (methods.IsAny())
                {
                    foreach (var method in methods)
                    {
                        var attr = method.GetCustomAttribute(typeof(NodeMethodAttribute));
                        if (attr.IsNotNull())
                        {
                            var targetAttr = attr as NodeMethodAttribute;
                            if (targetAttr.States.Contains(state) || targetAttr.States.Count == 0)
                            {
                                yield return (method, targetAttr.Name);
                            }
                        }
                    }
                }
            }
        }

        public static IEnumerable<WorkFlowNodeMethod> GetHandlerMethods(string key, string state)
        {
            var methods = GetMethods(key, state);
            foreach (var method in methods)
            {
                var parameters = method.Info.GetParameters();
                var paras = new Dictionary<string, string>();
                foreach (var para in parameters)
                {
                    paras.Add(para.Name, para.ParameterType.Name);
                }
                yield return new WorkFlowNodeMethod()
                {
                    Key = key,
                    State = state,
                    MethodLabel = method.Label,
                    MethodName = method.Info.Name,
                    Parameters = paras
                };
            }
        }
        public static object Excete(WorkFlowNodeMethod nodeMethod, IWorkSheetBase sheet, IWorkItemBase item,
        params object[] parameters)
        {
            try
            {
                var handler = handlers.First(p => p.Key == nodeMethod.Key).Value;
                handler = handler.CreateInstance(item, sheet);
                var methods = handler.GetType().GetMethods();
                foreach (var method in methods)
                {
                    if (method.IsPublic
                    && method.Name == nodeMethod.MethodName
                    && method.GetParameters().Length == parameters.Length)
                    {
                        var result = method.Invoke(handler, parameters);
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                var message = $"执行工作流节点{nodeMethod.Key}异常,{sheet.Id}/{item.Id}/{nodeMethod.MethodLabel}/{nodeMethod.State}";
                WorkFlowEngine.DataProvider.LogError(message, ex);
                return new { Result = false, Message = message, Error = ex.Message };
            }
            return null;
        }
    }
}
