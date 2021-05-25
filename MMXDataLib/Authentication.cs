using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Web.Security;
using System.IO;
using System.Diagnostics;

namespace MMXDataLib {
    public class Authentication {
        private int saltSize = 10;

        public string Salt { get; private set; }

        public bool NewUser { get; set; }

        public int MemberKey { get; private set; }

        public Authentication() {
            Salt = CreateSalt(saltSize);
        }

        public string EncryptPassword(string userPassword) {
            return CreatePasswordHash(userPassword, Salt);

        }

        public string TemporaryPassword {
            get { return System.Web.Security.Membership.GeneratePassword(8, 3); }
        }     


        private string CreateSalt(int size) {
            //Generate a cryptographic random number.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);

            // Return a Base64 string representation of the random number.
            return Convert.ToBase64String(buff);
        }

        private string CreatePasswordHash(string pwd, string salt) {

            string saltAndPwd = String.Concat(pwd, salt);
            byte[] pass = System.Text.Encoding.UTF8.GetBytes(saltAndPwd);
            byte[] hashedPwd = SHA256.Create().ComputeHash(pass);
            return Convert.ToBase64String(hashedPwd);
        }

        public bool DoesPasswordMatch(string password, string currentHashedPassword, string currentSalt) {

            string saltAndPwd = String.Concat(password, currentSalt);
            byte[] pass = System.Text.Encoding.UTF8.GetBytes(saltAndPwd);
            byte[] hashedPwd = SHA256.Create().ComputeHash(pass);
            int length = currentHashedPassword.Length;
            byte[] currentPasswordHash = Convert.FromBase64CharArray(currentHashedPassword.ToCharArray(), 0, length);
            return IsEqual(hashedPwd, currentPasswordHash);
        }

        private bool IsEqual(byte[] leftArray, byte[] rightArray) {
            bool isEqual = true;
            if (leftArray == null || rightArray == null) {
                isEqual = false;
            }
            else if (leftArray.Length != rightArray.Length) {
                isEqual = false;
            }
            else {
                for (int i = 0; i < leftArray.Length; i++) {
                    if (leftArray[i] != rightArray[i]) {
                        isEqual = false;
                        break;
                    }
                }

            }
            return isEqual;
        }

        // todo use email
        public bool IsValidUser(string email, string password, out string message) {
            Member member = MemberDataController.GetMember(email);
            
            if (member == null) {
                message = "Log-in incorrect, please try again.";
                return false;
            }
            Membership membership = MemberDataController.GetMembership (member.MemberKey);
            if (membership == null) {
                message = "Log-in incorrect, please try again.";
                return false;
            }
            else {
                if (!DoesPasswordMatch(password, membership.Password, membership.Salt)) {
                    message = "Log-in incorrect, please try again.";
                    return false;
                }
                else if (!membership.IsApproved) {
                    message = "Your account has not been approved. Contact your administrator.";
                    return false;
                }
                else if (membership.IsLocked) {
                    message = "Your account has been locked. Contact your administrator."; //+
                        //"You will NOT be able to login until you contact a site administrator and have your account unlocked.";
                    return false;
                }
                else {
                    message = "Access approved";
                    this.MemberKey = member.MemberKey;
                    return true;
                }            
            }
        }   

     
        

    }

        
    
}
