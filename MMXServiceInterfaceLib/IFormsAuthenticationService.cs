using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMXServiceInterfaceLib {
    public interface IFormsAuthenticationService {
        void SignIn(string email, bool createPersistentCookie);
        void SignOut();
    }
}
