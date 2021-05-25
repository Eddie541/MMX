using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMXDataService;
using MMXModelsLib;
using System.Text;

namespace MMXApplication.Controllers {
    //[HandleError]
    [LogAttribute]
    public abstract class BaseController : Controller {

        protected string ViewName { get; set; }

        public const string MxTab = "mxtab";
        public const string resultsTab = "resultstab";
        public const string classesTab = "classestab";
        public const string applicationTab = "applicationtab";
        public const string memberTab = "membertab";
        public const string scheduleTab = "scheduleTab";

        private string master = "~/Views/Shared/_Layout.cshtml";
        private string publicMaster = "~/Views/Shared/_Layout.cshtml";
        public string activeTabCssClass = "activetab";
        public string inActiveTabCssClass = "normaltab";


        protected string MasterPage {
            get {
                //if (IsValidUser) {
                //    return master;
                //} else {
                    return publicMaster;
                //}
            }
        }

        protected string PublicMasterPage {
            get {
                return publicMaster;
            }
        }

        protected int MemberKey {
            get {
                AccountMembershipService membershipService = new AccountMembershipService();
                System.Security.Principal.IIdentity principal = this.User.Identity;
                return membershipService.GetMemberKey(principal.Name);
            }
        }


        protected bool IsAdminRole {
            get {
                bool isAdmin = false;
                if (MemberKey > 0) {
                    MemberService memberService = new MemberService();
                    RoleModel r = memberService.GetMemberRole(MemberKey);
                    isAdmin = r.RoleKey < 3;
                }
                return isAdmin;
            }

        }

        protected string PrincipalName {
            get {
                System.Security.Principal.IIdentity principal = this.User.Identity;
                return principal.Name;
            }
        }

        protected bool PrincipalIsAdministrator {
            get {
                MemberService memberService = new MemberService();
                return memberService.IsAdministrator(MemberKey);
            }
        }

        protected bool PrincipalIsSuperUser {
            get {
                MemberService memberService = new MemberService();
                return memberService.IsSuperUser(MemberKey);
            }
        }

        protected bool IsValidUser {
            get {
                MemberService memberService = new MemberService();
                return memberService.IsValidUser(MemberKey);
            }
        }

        protected ViewResult BaseView(string viewName) {
            return View(viewName, MasterPage);
        }

        protected ViewResult BaseView(string viewName, object model) {
            return View(viewName, MasterPage, model);
        }

        protected ViewResult BaseView(string viewName, object model, string title) {
            ViewBag.Title = title;
            return View(viewName, MasterPage, model);
        }

        protected void SetTabClasses(string tabName) {
            SetTabsInactive();
            switch (tabName) {
                case MxTab:                    
                    ViewData[MxTab] = activeTabCssClass;
                    break;
                case resultsTab:
                    ViewData[resultsTab] = activeTabCssClass;
                    break;
                case classesTab:
                    ViewData[classesTab] = activeTabCssClass;
                    break;
                case applicationTab:
                    ViewData[applicationTab] = activeTabCssClass;
                    break;
                case memberTab:
                    ViewData[memberTab] = activeTabCssClass;
                    break;
                case scheduleTab:
                    ViewData[scheduleTab] = activeTabCssClass;
                    break;
            }
        }

        protected override void OnException(ExceptionContext filterContext) {
            string errorPage = "Error";
            //string master = IsValidUser ? "_Layout" : "_HomeLayout";
            SetErrorView(filterContext, errorPage, "_Layout");
        }

        private static void SetErrorView(ExceptionContext context, string view, string master) {
            var controllerName = context.RouteData.Values["controller"] as String;
            var actionName = context.RouteData.Values["action"] as String;
            var model = new HandleErrorInfo(context.Exception, controllerName, actionName);
            var result = new ViewResult {
                ViewName = view,
                MasterName = master,
                ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                TempData = context.Controller.TempData
            };
            // todo log error
            context.Result = result;
            context.ExceptionHandled = true;
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.StatusCode = 500;
            context.HttpContext.Response.TrySkipIisCustomErrors = true;
        }

        private void SetTabsInactive() {

            ViewData[MxTab] = inActiveTabCssClass;
            ViewData[resultsTab] = inActiveTabCssClass;
            ViewData[classesTab] = inActiveTabCssClass;
            ViewData[applicationTab] = inActiveTabCssClass;
            ViewData[memberTab] = inActiveTabCssClass;
            ViewData[scheduleTab] = inActiveTabCssClass;

        }


        protected bool ValidateModelState(BaseModel baseModel) {
            bool isValid = true;
            StringBuilder sb = new StringBuilder();
            if (ModelState.IsValid == false) {
                foreach (ModelState message in ModelState.Values) {
                    foreach (ModelError error in message.Errors) {
                        ErrorMessageModel model = new ErrorMessageModel() {
                            ErrorMessage = error.ErrorMessage
                        };
                       baseModel.Errors.Add(model);
                    }
                }
                isValid = false;
            } 
            return isValid;
        }
    }
}
