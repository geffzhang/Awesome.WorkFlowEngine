using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Walle.WorkFlowEngine.Common
{
    /// <summary>
    /// 封装单例类
    /// /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : class, new()
    {
        private static T _instance;
        private static object _syncRoot = new Object();

        /// <summary>
        /// 单例实例
        /// </summary>
        public static T Instance
        {
            get
            {
                var instance = _instance;
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        instance = Volatile.Read(ref _instance);
                        if (instance == null)
                        {
                            instance = new T();
                        }
                        Volatile.Write(ref _instance, instance);
                    }
                }
                return instance;
            }
        }
    }
}
