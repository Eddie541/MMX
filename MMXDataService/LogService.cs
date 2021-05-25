using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MMXModelsLib;
using MMXDataLib;
using MMXServiceInterfaceLib;


namespace MMXDataService { 

    public class LogService : ILogDataService {

        private readonly LogDataController _provider;     

        public LogService() {
            _provider = new LogDataController();
        }

        public void AddLogItem(string controller, string action, string exception, string result, string member) {
            _provider.AddLogItem(controller, action, exception, result, member);
        }

        public List<LogModel> GetLogModels(int take) {
            List<AppLog> logItems = _provider.GetLogItems(take);
            return CreateLogModels(logItems);
        }

        public List<LogModel> GetLogModelsWhere(FindTransactionModel model) {
            List<AppLog> logItems = _provider.GetLogItemsWhere(model.MemberName, model.StartDate, model.EndDate, model.HasException);
            return CreateLogModels(logItems);
        }

        private static List<LogModel> CreateLogModels(List<AppLog> logItems) {
            List<LogModel> logModels = new List<LogModel>();
            foreach (AppLog log in logItems) {
                LogModel model = new LogModel() {
                    LogKey = log.LogKey,
                    Controller = log.Controller,
                    Action = log.ActionName,
                    Result = log.Result,
                    ExceptionMessage = log.ExceptionMessage,
                    LogDate = log.LogDateTime,
                    Member = log.Member

                };
                logModels.Add(model);
            }
            return logModels;
        }

     
    }
}