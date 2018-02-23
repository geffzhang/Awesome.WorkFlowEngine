using System;
using Walle.WorkFlowEngine.Core;

namespace Walle.WorkFlowEngine.Interface
{
    public interface IDataProvider
    {
        /// <summary>
        /// 提交工作流流转
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        bool Commit(IWorkSheetBase sheet);
        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        string LogInfo(IWorkSheetBase sheet, string message,string currentUser,string workItemId="");

        void LogError(string message,Exception ex);
        
        IWorkSheetBase Query(string sheetId);

        /// <summary>
        /// 结束并转移工单
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        bool Final(IWorkSheetBase sheet);

        string GetCurrentUser(IWorkSheetBase sheet, WorkFlowActorUser user);

        bool CheckRequire(IWorkSheetBase sheet, IWorkItemBase item, WorkFlowNodeRequire require);
    }
}