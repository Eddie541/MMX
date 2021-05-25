using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESS.UtilitiesLib;

namespace MMXDataLib {
    public class LogDataController : BaseDataController {        

        public LogDataController() {
        }

        public void AddLogItem(string controller, string action, string exception, string result, string member) {
            
            AppLog logItem = new AppLog();
            logItem.Controller = controller;
            logItem.ActionName = action;
            logItem.ExceptionMessage = exception;
            logItem.Result = result;
            logItem.LogDateTime = DateTime.Now;
            logItem.Member = member;
            try {                
                context.AppLogs.AddObject(logItem);
                context.SaveChanges();

            }
            catch (System.Data.EntityException) {
                // todo write to log file
            }
        }

        public List<AppLog> GetLogItems(int take) {
            return context.AppLogs.OrderByDescending(hc => hc.LogKey).Take(take).ToList();
        }

        public List<AppLog> GetLogItemsWhere(string memberName, DateTime startDate, DateTime endDate, bool hasException) {

            IEnumerable<AppLog> logItems = context.AppLogs.Where(hc => hc.LogDateTime >= startDate && hc.LogDateTime <= endDate);
            if (string.IsNullOrEmpty(memberName) == false) {
                logItems = logItems.Where(hc => hc.Member.Contains(memberName, StringComparison.OrdinalIgnoreCase));
            }
            if (hasException) {
                logItems = logItems.Where(hc => string.IsNullOrEmpty(hc.ExceptionMessage) == false);
            }           
            return logItems.ToList();
        }


    }
}
