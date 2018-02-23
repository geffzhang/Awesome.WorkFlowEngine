using Walle.WorkFlowEngine.Common;
using Walle.WorkFlowEngine.Core;
using Walle.WorkFlowEngine.WorkNode.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Walle.WorkFlowEngine.WorkNode
{
    internal static partial class NodeContainer
    {
        internal static List<WorkFlowNode> NodeList = new List<WorkFlowNode>();

        internal static string Load<T>()
        {
            try
            {
                Type type = typeof(T);
                var attrs = type.GetCustomAttributes();
                var (key, name) = attrs.GetKeyName();

                var members = type.GetMembers();
                if (!members.HasAny())
                {
                    throw new Exception($"无法获取处理器{key}/{name}的成员信息,加载失败.");
                }
                var stateMember = members.GetStateMember();
                var configMember = members.GetConfigMember();

                //获取状态集
                var (start, end) = stateMember.GetStartEnd();
                var states = stateMember.GetEnumList();

                //获取配置集
                var configs = configMember.GetEnumList();

                WorkFlowNode node = BuildWorkFlowNode(key, name, start, end, states, configs);
                if (node.Valicate(out string message))
                {
                    NodeList.Add(node);
                    return key;
                }
                else
                {
                    throw new Exception(message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        private static WorkFlowNode BuildWorkFlowNode(string key, string name, string start, string end, List<(string state, string label)> states, List<(string state, string label)> configs)
        {
            WorkFlowNode node = new WorkFlowNode();
            if (configs.HasAny())
            {
                foreach (var cfg in configs)
                {
                    node.Config.Add(new WorkFlowNodeSettings()
                    {
                        Label = cfg.label,
                        Key = cfg.state
                    });
                }
            }
            if (states.HasAny())
            {
                foreach (var state in states)
                {
                    node.Status.Add(new WorkFlowNodeStatus()
                    {
                        Label = state.label,
                        Value = state.state
                    });
                }
            }

            node.BeginStatus = node.Status.First(p => p.Value == start);
            node.EndStatus = node.Status.First(p => p.Value == end);
            node.Key = key;
            node.Name = name;
            return node;
        }

        private static bool Valicate(this WorkFlowNode node, out string message)
        {
            if (string.IsNullOrWhiteSpace(node.Key))
            {
                message = " Handler has no KeyAttributes ,or KeyValue is empty";
                return false;
            }
            if (string.IsNullOrWhiteSpace(node.Name))
            {
                message = " Handler has no KeyAttributes ,or NameValue is empty";
                return false;
            }
            if (node.Status == null || !node.Status.Any())
            {
                message = " Handler has no EnumType marked with StatusAttributes ,or there are no EnumItem Marked.";
                return false;
            }
            if (node.BeginStatus == null)
            {
                message = " Handler has no BeginStatus marked in your Status enum";
                return false;
            }
            if (node.EndStatus == null)
            {
                message = " Handler has no BeginStatus marked in your Status enum";
                return false;
            }
            message = "checked ok";
            return true;
        }
        private static (string key, string name) GetKeyName(this IEnumerable<Attribute> attrs)
        {
            var key = default(string);
            var name = default(string);
            foreach (var attr in attrs)
            {
                if (attr.GetType() == typeof(KeyAttribute))
                {
                    var target = (attr as KeyAttribute);
                    key = target?.KeyValue;
                    name = target?.NameValue;
                    return (key, name);
                }
            }
            return (key, name);
        }

        private static MemberInfo GetStateMember(this MemberInfo[] members)
        {
            foreach (var member in members)
            {
                var attrs = member.GetCustomAttributes();
                if (attrs.Any(attr => attr.GetType() == typeof(StateAttribute)))
                {
                    return member;
                }
            }
            return null;
        }
        private static MemberInfo GetConfigMember(this MemberInfo[] members)
        {
            foreach (var member in members)
            {
                var attrs = member.GetCustomAttributes();
                if (attrs.Any(attr => attr.GetType() == typeof(ConfigAttribute)))
                {
                    return member;
                }
            }
            return null;
        }
        
        private static List<(string state, string label)> GetEnumList(this MemberInfo member)
        {
            if (member.IsNull())
            {
                return new List<(string state, string label)>();
            }

            var result = new List<(string state, string label)>();
            var type = member as Type;

            var members = type.GetMembers();
            foreach (var sub in members)
            {
                var attrs = sub.GetCustomAttributes();
                foreach (var attr in attrs)
                {
                    if (attr is LabelAttribute)
                    {
                        var label = (attr as LabelAttribute).Label;
                        var state = sub.Name;
                        result.Add((state, label));
                    }
                }
            }
            return result;
        }
        private static (string start, string end) GetStartEnd(this MemberInfo member)
        {
            var start = default(string);
            var end = default(string);

            Type type = member as Type;

            var members = type.GetMembers();

            // var members = member.GetMembers();

            foreach (var sub in members)
            {
                var attrs = sub.GetCustomAttributes();
                if (attrs.Any(attr => attr.GetType() == typeof(StartAttribute)))
                {
                    start = sub.Name;
                }
                if (attrs.Any(attr => attr.GetType() == typeof(FinishAttribute)))
                {
                    end = sub.Name;
                }
            }

            return (start, end);
        }

    }
}