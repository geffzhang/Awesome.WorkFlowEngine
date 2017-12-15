using System;
using System.Collections.Generic;
using System.Text;

namespace Awesome.WorkFlowEngine.Events
{
    public static class TransferEventPublisher
    {
 
        private static event Action<TransferEventArgs> On_Transfered;
 
        public static void Invoke(TransferEventArgs args)
        {
            On_Transfered?.BeginInvoke(args, _ =>
            {

            }, null);
        }

        public static void Register(Action<TransferEventArgs> action)
        {
            On_Transfered += action;
        }

        public static void UnRegister(Action<TransferEventArgs> action)
        {
            On_Transfered += action;
        }
    }
}
