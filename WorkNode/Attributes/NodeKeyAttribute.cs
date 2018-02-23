using System;
using System.Collections.Generic;
using System.Text;

namespace Walle.WorkFlowEngine.WorkNode.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class KeyAttribute: Attribute
    {
        public string KeyValue { get; set; } = string.Empty;

        public string  NameValue {get;set;} = string.Empty;
        public KeyAttribute(string key,string name)
        {
            this.KeyValue= key.ToLower();
            this.NameValue = name;
        }
    }
}
