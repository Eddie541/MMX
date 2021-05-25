using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace MMXServiceInterfaceLib {
    public interface IMembershipService {
        int MinPasswordLength { get; }

        bool ValidateUser(string email, string password, out string errorMessage);
        MembershipCreateStatus CreateUser(string email);
        bool ChangePassword(string email, string oldPassword, string newPassword);
        bool ChangeEmail(string userName, string password, string newEmail);
        bool HasTemporaryPassword(string email);
        string ResetPassword(string emailAddress);
        bool AdminResetPassword(int memberKey);
        void LockMember(int memberKey);
        void UnLockMember(int memberKey);
        string GetMemberName(int memberKey);
        
        
    }
}
