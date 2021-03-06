using System;
using System.Web;
using MMXServiceInterfaceLib;
using System.Web.Security;

namespace MMXDataService {
    public class FormsAuthenticationService : IFormsAuthenticationService {
        public void SignIn(string userName, bool createPersistentCookie) {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");

            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SignOut() {
            FormsAuthentication.SignOut();
        }
    }
}
