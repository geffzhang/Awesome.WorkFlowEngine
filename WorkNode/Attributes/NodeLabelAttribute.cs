using System;
using System.Collections.Generic;
using System.Text;

namespace Awesome.WorkFlowEngine.WorkNode.Attributes
{
    public class LabelAttribute : Attribute
    {
        public string Label { get; set; } = string.Empty;
        public LabelAttribute(string label)
        {
            this.Label = label;
        }
    }
}
