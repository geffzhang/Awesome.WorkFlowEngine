using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Awesome.WorkFlowEngine.Common;

namespace Awesome.WorkFlowEngine.WorkNode.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class NodeMethodAttribute : Attribute
    {
        public NodeMethodAttribute(string name, params Object[] states)
        {
            this.Name = name;
            if (states.IsNotNull())
            {
                foreach (var temp in states)
                {
                    var value = temp.ToString();
                    States.Add(value);
                }
            }
        }

        public string Name { get; set; }
        public List<string> States = new List<string>();
    }
}
