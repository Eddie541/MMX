using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using MMXModelsLib;
using MMXServiceInterfaceLib;
using MMXDataService;

namespace MMXApplication.Controllers {
    public class AccountController : BaseController {


        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }
        public IContentService ContentService { get; set; }

        protected override void Initialize(RequestContext requestContext) {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }


            base.Initialize(requestContext);
        }

        //
        // GET: /Account/LogOn

        public ActionResult LogOn() {
            LogOnModel model = new LogOnModel() {
                EmailSentNotice = "",
                Password = "",
                RememberMe = false,
                ShowEmailSentNotice = false,
                UserEmail = ""
            };

            return PartialView("LogOn", model);
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl) {

           //var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray(); 
           
            if (ModelState.IsValid) {
                //if (Membership.ValidateUser(model.UserEmail, model.Password)) {
                //    FormsAuthentication.SetAuthCookie(model.UserEmail, model.RememberMe);
                //    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                //        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\")) {
                //        return Redirect(returnUrl);
                //    } else {
                //        return RedirectToAction("Index", "Home");
                //    }
                //} else {
                //    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                //}
                string errorMessage = "";
                if (MembershipService.ValidateUser(model.UserEmail, model.Password, out errorMessage)) {
                    FormsService.SignIn(model.UserEmail, model.RememberMe);
                    if (!String.IsNullOrEmpty(returnUrl)) {
                        return Redirect(returnUrl);
                    } else {
                        if (MembershipService.HasTemporaryPassword(model.UserEmail)) {
                            return RedirectToAction("ChangePassword");
                        } else {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                } else {
                    ModelState.AddModelError("", errorMessage); //"The user name or password provided is incorrect.");
                    this.TempData.Add("logEnteredName", model.UserEmail);
                }
            }

            // If we got this far, something failed, redisplay form
            return PartialView("LogOn", model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff() {
            FormsAuthentication.SignOut();
            //LogOnModel model = new LogOnModel() {
            //    EmailSentNotice = "",
            //    Password = "",
            //    RememberMe = false,
            //    ShowEmailSentNotice = false,
            //    UserEmail = ""
            //};
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register() {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return BaseView("Register");
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model) {
            //if (ModelState.IsValid) {
            //    // Attempt to register the user
            //    MembershipCreateStatus createStatus;
            //    Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

            //    if (createStatus == MembershipCreateStatus.Success) {
            //        FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
            //        return RedirectToAction("Index", "Home");
            //    } else {
            //        ModelState.AddModelError("", ErrorCodeToString(createStatus));
            //    }
            //}

            //// If we got this far, something failed, redisplay form
            //return View(model);
            if (ModelState.IsValid) {
                // Attempt to register the user
                MembershipCreateStatus createStatus = MembershipService.CreateUser(model.Email);
                if (createStatus == MembershipCreateStatus.Success) {
                    FormsService.SignIn(model.Email, false /* createPersistentCookie */);

                    string ms = "A temporary password has been sent to email address " + model.Email;
                    return RedirectToAction("LogOn", "Account", new { message = ms });
                } else {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View("LogOn", PublicMasterPage);
        }

        [Authorize]
        public ActionResult ChangeEmail() {
            SetTabClasses(memberTab);     
            return BaseView("ChangeEmail");
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangeEmail(ChangeEmailModel model) {
            SetTabClasses(memberTab);     
            MembershipService.ChangeEmail(PrincipalName, model.Password, model.Email);
            //todo show email success
            return RedirectToAction("Member", "Member");
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword() {
            SetTabClasses(memberTab);     
            return BaseView("ChangePassword");
        }

        
        public ActionResult ResetPassword() {
            return BaseView("ResetPassword");
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModel model) {
            if (ModelState.IsValid) {
                // todo
            }
             
            return View("LogOn", PublicMasterPage);
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model) {
               
            //if (ModelState.IsValid) {

            //    // ChangePassword will throw an exception rather
            //    // than return false in certain failure scenarios.
            //    bool changePasswordSucceeded;
            //    try {
            //        MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
            //        changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
            //    } catch (Exception) {
            //        changePasswordSucceeded = false;
            //    }

            //    if (changePasswordSucceeded) {
            //        return RedirectToAction("ChangePasswordSuccess");
            //    } else {
            //        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
            //    }
            //}

            //// If we got this far, something failed, redisplay form
            //return View(model);
            if (ModelState.IsValid) {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword)) {
                    // todo show password success
                    return RedirectToAction("Member", "Member"); 
                } else {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return BaseView("ChangePassword", model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess() {
            return View();
        }

        //#region Status Codes
        //private static string ErrorCodeToString(MembershipCreateStatus createStatus) {
        //    // See http://go.microsoft.com/fwlink/?LinkID=177550 for
        //    // a full list of status codes.
        //    switch (createStatus) {
        //        case MembershipCreateStatus.DuplicateUserName:
        //            return "User name already exists. Please enter a different user name.";

        //        case MembershipCreateStatus.DuplicateEmail:
        //            return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

        //        case MembershipCreateStatus.InvalidPassword:
        //            return "The password provided is invalid. Please enter a valid password value.";

        //        case MembershipCreateStatus.InvalidEmail:
        //            return "The e-mail address provided is invalid. Please check the value and try again.";

        //        case MembershipCreateStatus.InvalidAnswer:
        //            return "The password retrieval answer provided is invalid. Please check the value and try again.";

        //        case MembershipCreateStatus.InvalidQuestion:
        //            return "The password retrieval question provided is invalid. Please check the value and try again.";

        //        case MembershipCreateStatus.InvalidUserName:
        //            return "The user name provided is invalid. Please check the value and try again.";

        //        case MembershipCreateStatus.ProviderError:
        //            return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

        //        case MembershipCreateStatus.UserRejected:
        //            return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

        //        default:
        //            return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
        //    }
        //}
        //#endregion
    }
}
