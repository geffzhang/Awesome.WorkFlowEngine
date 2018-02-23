using Walle.WorkFlowEngine.Interface;
using Walle.WorkFlowEngine.WorkNode.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Walle.WorkFlowEngine.Core
{

    public class WorkFlowNodeRequire
    {
        public string Key { get; set; } = string.Empty;
        public INodeHandler Handler { get; set; } = null;
        public List<string> RequrieName { get; set; } = new List<string>();
        public WorkFlowNodeRequire()
        {

        }

        public WorkFlowNodeRequire Load(INodeHandler handler)
        {
            this.Handler = handler;
            this.Key = this.Handler.GetType().GetCustomAttribute<KeyAttribute>()?.KeyValue;
            this.RequrieName = this.Handler.GetType().GetCustomAttribute<NodeRequireAttribute>()?.Names;
            if (string.IsNullOrEmpty(Key) || this.RequrieName == null || !this.RequrieName.Any())
            {
                return null;
            }
            return this;
        }
    }
}
