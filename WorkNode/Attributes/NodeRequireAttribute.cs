using System;
using System.Collections.Generic;

namespace Awesome.WorkFlowEngine.WorkNode.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NodeRequireAttribute : Attribute
    {
        public List<string> Names = new List<string>();
        public NodeRequireAttribute(params string[] names )
        {
            foreach (var item in names)
            {
                Names.Add(item);
            }
        }
    }
}