using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMXModelsLib;

namespace MMXApplication.Controllers {
    public class HomeController : BaseController {
        public HomeController() {
            ViewName = "Index";
            SetTabClasses(MxTab);
        }

        
        public ActionResult Index() {            
            ViewBag.Message = "MMX";
            SetTabClasses(MxTab);
            return BaseView(ViewName);
        }

        public ActionResult About() {
            SetTabClasses(scheduleTab);
            return BaseView("About");
        }
    }
}
