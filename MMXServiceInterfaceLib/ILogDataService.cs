using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMXModelsLib;

namespace MMXServiceInterfaceLib {
    public interface ILogDataService {
        void AddLogItem(string controller, string action, string exception, string result, string member);
        List<LogModel> GetLogModels(int take);
        List<LogModel> GetLogModelsWhere(FindTransactionModel model);
    }
}
