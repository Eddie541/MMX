using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Security;
using System.Net.Mail;
using MMXServiceInterfaceLib;
using MMXDataLib;

namespace MMXDataService {
    public class AccountMembershipService : IMembershipService {

        private readonly MemberDataController _provider;

        public AccountMembershipService()
            : this(null) {
        }

        public AccountMembershipService(MembershipProvider provider) {

            _provider = new MemberDataController();
        }

        public int MinPasswordLength {
            get {
                return _provider.MinPasswordLength;
            }
        }

        public bool ValidateUser(string email, string password, out string errorMessage) {
            return _provider.AuthenticateUser(email, password, out errorMessage);
        }

        public MembershipCreateStatus CreateUser(string email) {

            MembershipCreateStatus status = ValidateNewUser(email);

            if (status == MembershipCreateStatus.Success) {
                _provider.CreateMember(email);
            }
            return status;
        }

        private MembershipCreateStatus ValidateNewUser(string email) {
            MembershipCreateStatus createStatus = MembershipCreateStatus.Success;
            if (String.IsNullOrEmpty(email)) throw new ArgumentException("Value cannot be null or empty.", "email");
            try {
                MailAddress address = new MailAddress(email);
                string t = address.Host;
            }
            catch (ArgumentException) {
                createStatus = MembershipCreateStatus.InvalidEmail;
            }
            catch (FormatException) {
                createStatus = MembershipCreateStatus.InvalidEmail;
            }
            if (_provider.MemberEmailIsUnique(email) == false) {
                createStatus = MembershipCreateStatus.DuplicateEmail;
            }

            // todo check valid email
            return createStatus;
        }

        public bool ChangePassword(string email, string oldPassword, string newPassword) {
            if (String.IsNullOrEmpty(email)) throw new ArgumentException("Value cannot be null or empty.", "email");
            if (String.IsNullOrEmpty(oldPassword)) throw new ArgumentException("Value cannot be null or empty.", "oldPassword");
            if (String.IsNullOrEmpty(newPassword)) throw new ArgumentException("Value cannot be null or empty.", "newPassword");

            try {
                int currentUserKey = MemberDataController.GetMemberKey(email);
                if (currentUserKey > 0) {
                    return MemberDataController.ChangePassword(oldPassword, newPassword, currentUserKey);
                }
                else {
                    return false;
                }
            }
            catch (ArgumentException) {
                return false;
            }
            catch (MembershipPasswordException) {
                return false;
            }
        }

        public bool ChangeEmail(string email, string password, string newEmail) {
            try {
                int currentUserKey = MemberDataController.GetMemberKey(email);
                if (currentUserKey > 0) {
                    return MemberDataController.ChangeEmail(password, newEmail, currentUserKey);
                }
                else {
                    return false;
                }
            }
            catch (ArgumentException) {
                return false;
            }
            catch (MembershipPasswordException) {
                return false;
            }

        }


        public bool HasTemporaryPassword(string email) {
            return MemberDataController.HasTemporaryPassword(email);
        }

        public int GetMemberKey(string memberName) {
            return MemberDataController.GetMemberKey(memberName);
        }

        public string GetMemberName(int memberKey) {
            return MemberDataController.GetMemberName(memberKey);
        }

        public string ResetPassword(string emailAddress) {
            string message = string.Format("Cannot find match for a user with an email address of {0} ", emailAddress);
            Member member = null;
            try {
                member = MemberDataController.GetMember(emailAddress);
                if (member.Email.ToLowerInvariant() == emailAddress.ToLowerInvariant()) {
                    if (MemberDataController.ResetMemberPassword(member.MemberKey)) {
                        message = "A temporary password has been sent to email address " + member.Email;
                    }
                    else {
                        message = "An attempt to send an email with a temporary password failed,\n\rPlease contact the site administrator";
                    }
                }
            }
            catch (Exception) { }
            return message;
        }

        public bool AdminResetPassword(int memberKey) {
            return MemberDataController.ResetMemberPassword(memberKey);
        }

        public void LockMember(int memberKey) {
            _provider.LockMember(memberKey);
        }

        public void UnLockMember(int memberKey) {
            _provider.UnLockMember(memberKey);
        }
    }  
    
}
