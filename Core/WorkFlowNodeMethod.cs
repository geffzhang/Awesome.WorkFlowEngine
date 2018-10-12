
using System.Collections;
using System.Collections.Generic;

namespace Walle.WorkFlowEngine.Core
{
    public class WorkFlowNodeMethod
    {
        public string Key {get;set;} = string.Empty;
        public string State {get;set;} = string.Empty;
        public string MethodLabel { get; set; } = string.Empty;
        public string MethodName { get; set; } = string.Empty;
        public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();
    }
}