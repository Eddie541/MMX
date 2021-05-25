using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using MMXDataService;


namespace MMXApplication.Controllers {
    public class LogAttribute : ActionFilterAttribute {

        LogService service;
        private LogService Logger {
            get {
                if (service == null) {
                    service = new LogService();
                }
                return service;
            }
            set { service = value; }
        }

        public LogAttribute() {
            
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext) {
            string controller = filterContext.Controller.ToString();
            string action = filterContext.ActionDescriptor.ActionName;
            StringBuilder exceptionSb = new StringBuilder();
            if (filterContext.Exception != null) {
                exceptionSb.AppendLine("Exception");
                exceptionSb.Append(filterContext.Exception.Message);
                exceptionSb.Append(filterContext.Exception.StackTrace);
                Exception innerException = filterContext.Exception.InnerException;
               
                while (innerException != null) {
                    exceptionSb.Append(filterContext.Exception.InnerException.Message);
                    exceptionSb.Append(filterContext.Exception.InnerException.StackTrace);
                    innerException = innerException.InnerException;
                }
            }
            string exception = exceptionSb.Length == 0 ? null : exceptionSb.ToString();
            StringBuilder routeSb = new StringBuilder();
            IDictionary<string, object> hh = filterContext.RouteData.Values;
            foreach (KeyValuePair<string, object> routeValues in hh) {
                routeSb.Append(routeValues.Key);
                routeSb.Append(routeValues.Value); 
            }           
            
            string result = routeSb.Length == 0 ? null : routeSb.ToString();
            string member = filterContext.HttpContext.User.Identity.Name;
            if (string.IsNullOrEmpty(member)) {
                if (filterContext.Controller.TempData.ContainsKey("logEnteredName")) {
                    object nameValue = null;
                    if (filterContext.Controller.TempData.TryGetValue("logEnteredName", out nameValue)) {
                        member = nameValue.ToString();
                    }
                }
                if (filterContext.Controller.TempData.ContainsKey("logEmail")) {
                    object emailValue = null;
                    if (filterContext.Controller.TempData.TryGetValue("logEmail", out emailValue)) {
                        member += "  " + emailValue.ToString();
                    }
                }
                filterContext.Controller.TempData.Clear();
            }
            Logger.AddLogItem(controller, action, exception, result, member);
            
           
        }

            
        
    }
}