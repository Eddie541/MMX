using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMXDataLib {
    public abstract class BaseDataController {

        protected static MMXEntities context;

        protected List<string> ErrorCollection { get; set; }        

        protected Action MethodAction;

        public BaseDataController(string testConnection = "") {
           if (string.IsNullOrEmpty(testConnection)) {
               context = MMXDataContext.DataContext;
           } else {
               context = new MMXEntities(testConnection);
           }
           ErrorCollection = new List<string>();
           
        }

        //protected abstract void SetErrorMessage();
    }
}
