using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MMXModelsLib {

    public class AdminLogModel : BaseModel {
        public int CurrentLogCount { get; set; }
        private List<LogModel> _logModels;
        public List<LogModel> LogModels {
            get { return _logModels; }
            set {
                _logModels = value;
                CurrentLogCount = _logModels.Count;
            }
        }
    }
    

    public class LogModel : BaseModel {
        public int LogKey { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string ExceptionMessage { get; set; }
        public string Result { get; set; }
        public DateTime LogDate { get; set; }
        public string Member { get; set; }        
    }

    public class FindTransactionModel : BaseModel {
        [DisplayName("Member Name:")]        
        public string MemberName { get; set; }

        [DisplayName("Start Date:")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DisplayName("End Date:")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [DisplayName("Has Exception:")]
        public bool HasException { get; set; }

    }
}
