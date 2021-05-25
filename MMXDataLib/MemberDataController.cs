using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Transactions;
using System.Text;

namespace MMXDataLib {
    public class MemberDataController : BaseDataController {

        public enum PasswordType {
            Temporary,
            Valid
        }

        public int MinPasswordLength {
            get {
                int minLength = 4;
                object o = AdminDataController.GetConfiguration("minPasswordLength");
                if (o != null) {
                    Int32.TryParse(o.ToString(), out minLength);
                }
                return minLength;
            }
        }


        //static MMXEntities context;
        //
        public MemberDataController(string testConn = "") :
            base(testConn) {
        }

        
        private bool HasMember(string memberEmail) {
            return context.Members.Any(m => m.Email.ToLower().Equals(memberEmail.ToLower()));
        }

        public static Member GetMember(string memberEmail) {
            return context.Members.Where(m => m.Email.ToLower().Equals(memberEmail.ToLower())).Single();

        }

        public static Member GetMember(int memberKey) {
            return context.Members.Where(m => m.MemberKey == memberKey).SingleOrDefault();

        }

        public static Role GetMemberRole(string memberEmail) {
            if (string.IsNullOrEmpty(memberEmail) == false) {
                Member m = GetMember(memberEmail);
                Role r = null;
                if (m != null) {
                    r = m.Role;
                }
                return r;
            } else {
                return null;
            }
        }

        public Role GetMemberRole(int memberKey) {

            Member m = GetMember(memberKey);
            Role r = null;
            if (m != null) {
                r = m.Role;
            }
            return r;
        }          
                
        public static int GetMemberKey(string memberEmail) {
            int memberKey = 0;
            try {
                memberKey = context.Members.Where(m => m.Email.ToLower().Equals(memberEmail.ToLower())).Single().MemberKey;
            } catch (Exception) {
            }
            return memberKey;
        }

        private static int GetMemberKeyByName(string lastName, string firstName) {
            int memberKey = 0;
            try {
                Member member = context.Members.Where(m => m.LastName.Equals(lastName, StringComparison.InvariantCultureIgnoreCase) && m.FirstName.Equals(firstName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if (member != null) {
                    memberKey = member.MemberKey;
                }
            } catch (Exception) {
            }
            return memberKey;
        }

        public static string GetMemberName(int memberKey) {
            StringBuilder memberName = new StringBuilder();
            try {
                Member member = GetMember(memberKey);
                if (member != null) {
                    memberName.Append(member.FirstName + " ");
                    if (string.IsNullOrEmpty(member.MiddleName) == false) {
                        memberName.Append(member.MiddleName + " ");
                    }
                    memberName.Append(member.LastName);
                    if (string.IsNullOrEmpty(member.Suffix) == false) {
                        memberName.Append("  " + member.Suffix);
                    }
                }
            }
            catch (Exception) {
            }
            return memberName.ToString();
            ;
        }

        public static Membership GetMembership(int memberKey) {
            Membership membership = null;
            if (memberKey > 0) {
                try {
                    membership = context.Memberships.Where(mb => mb.MemberKey == memberKey).Single();
                } catch {

                }
            }
            return membership;

        }

        public MemberSummary GetMemberSummary(int memberKey) {
            return context.MemberSummaries.Where(mb => mb.MemberKey == memberKey).Single();

        }

        public IEnumerable<MemberSummary> GetMemberSummaries() {
            return context.MemberSummaries.Select(ms => ms);
        }

        //private void SetLastActivity(int memberKey) {
        //    Member m = GetMember(memberKey);
        //    m.LastActivityDate = DateTime.Now;
        //    context.SaveChanges();
        //}


        public bool AuthenticateUser(string email, string password, out string errorMessage) {
            int currentUserKey = 0;
            bool isValidUser = false;
            if (HasMember(email)) {
                Authentication authentication = new Authentication();
                if (authentication.IsValidUser(email, password, out errorMessage)) {
                    isValidUser = true;
                    currentUserKey = authentication.MemberKey;
                    SetFailedPasswordAttempts(isValidUser, currentUserKey);
                    //SetLastActivity(currentUserKey);
                }
            } else {
                errorMessage = "Cannot validiate member '" + email + "'";
            }
            errorMessage = "Cannot validiate member '" + email + "'";
            return isValidUser;
        }

        public void SetFailedPasswordAttempts(bool isReset, int memberKey) {           
            try {
                Membership membership = GetMembership(memberKey);
                Member memberUser = GetMember(memberKey);
                if (isReset) {
                    membership.LastLoginDate = DateTime.Now;
                    membership.FailedPasswordAttemptCount = 0;
                    membership.FailedPasswordAnswerAttemptCount = 0;
                }
                else {
                    membership.FailedPasswordAttemptCount++;
                }
                //memberUser.LastActivityDate = DateTime.Now;

                context.SaveChanges();

            }
            catch (System.Data.OptimisticConcurrencyException cce) {
                throw cce;
            }
            catch (Exception ex) {
                throw ex;
            }

        }


        public int CreateTemporaryMember(string firstName, string lastName, string className) {
            int memberKey = GetMemberKeyByName(lastName, firstName);

            if (memberKey != 0) {
                return memberKey;
            }

            Member member = new Member();
            member.RoleKey = 5;
            member.LastName = lastName;
            member.FirstName = firstName;
            member.DateOfBirth = SetTemporaryMemberDoB(className);
            context.Members.AddObject(member);
            context.SaveChanges();
            memberKey = member.MemberKey;
            
            return memberKey;
        }


        private DateTime SetTemporaryMemberDoB(string className) {
            DateTime birthDate = DateTime.Now;
            int bornYearsAgo = 15;
            int maxYearsAgo = 29;
            switch (className) {
                case "Beginner":
                case "Women":
                case "250C":
                    bornYearsAgo = CreateRandomValue(bornYearsAgo, maxYearsAgo);
                    break;
                case "Open A":                   
                case "Open B":                   
                case "Open C":
                       bornYearsAgo = CreateRandomValue(19, maxYearsAgo);                   
                    break;
                case "30+A":
                    bornYearsAgo = 30;
                    break;
                case "35+A":
                case "35+B":
                case "35+C":
                    bornYearsAgo = 35;
                    break;
                case "40+A":
                case "40+B":
                    bornYearsAgo = 40;
                    break;
                case "45+A":
                case "45+B":
                    bornYearsAgo = 45;
                    break;
                case "50+A":
                case "50+B":
                    bornYearsAgo = 50;
                    break;
                case "55+":
                    bornYearsAgo = 55;
                    break;
                case "60+":
                    bornYearsAgo = 60;
                    break;
            }

            return birthDate.AddYears(-bornYearsAgo);


        }

        private int CreateRandomValue(int min, int max) {
            Random rand = new Random(DateTime.Now.Millisecond);
            return rand.Next(min, max); 
        }


        private bool DoesMemberExist(string firstName, string lastName, string middleName = null, string suffix = null) {
            int count = context.Members.Count(m => m.LastName == lastName && m.FirstName == firstName);
            return count > 0;

        }



        public bool CreateMember(string email, string firstName = null,
                    string middleName = null, string lastName = null, string suffix = null, string phone = null,
                    string licenseNumber = null, DateTime? dateOfBirth = null, int addressKey = 0) {
            bool success = false;
            DateTime createDate = DateTime.Now;    

            Member member = new Member();
            member.Email = email;
            // todo allow for data entry user (4)
            member.RoleKey = 3;
            //member.LastActivityDate = createDate;

            if (firstName != null) {
                member.FirstName = firstName;
            }
            if (middleName != null) {
                member.MiddleName = middleName;
            }
            if (lastName != null) {
                member.LastName = lastName;
            }
            if (phone != null) {
                member.Phone = phone;
            }
            if (licenseNumber != null) {
                member.LicenseNumber = licenseNumber;
            }
            if (dateOfBirth != null) {
                member.DateOfBirth = dateOfBirth;
            }
            if (addressKey > 0) {
                member.AddressKey = addressKey;
            }


            Authentication auth = new Authentication();
            string tempPassword = auth.TemporaryPassword;

            Membership membership = new Membership();
            membership.Password = auth.EncryptPassword(tempPassword);
            membership.Salt = auth.Salt;
            membership.PasswordQuestion = "";
            membership.PasswordAnswer = "";
            membership.IsApproved = true;
            membership.IsLocked = false;
            membership.CreateDate = createDate;
            membership.LastLoginDate = createDate;
            membership.LastPasswordChangeDate = createDate;
            membership.LastLockoutDate = createDate.AddDays(-1);
            membership.FailedPasswordAttemptCount = 0;
            membership.FailedPasswordAttemptWindowStart = createDate;
            membership.FailedPasswordAttemptCount = 0;
            membership.FailedPasswordAttemptWindowStart = createDate;
            membership.LoweredEmail = email.ToLower();
            membership.Comment = "";
            membership.PasswordType = (int)PasswordType.Temporary;


            try {

                using (TransactionScope scope = new TransactionScope()) {

                    context.Members.AddObject(member);
                    context.SaveChanges();

                    membership.MemberKey = member.MemberKey;
                    context.Memberships.AddObject(membership);
                    context.SaveChanges();

                    //SendTempPassword(tempPassword, email, memberName);
                    scope.Complete();
                    success = true;
                }
            }
            catch (Exception e) {
                // TODO: handle this and show 
                // contact support message - 
                // it got this far - 
                // send notification email to admin
                throw e;
            }

            return success;

        }

        //public bool MemberNameIsUnique(string memberName) {
        //    IEnumerable<Member> members = context.Members.Where(m => m.Name.Equals(memberName));
        //    return members.Count() == 0;
        //}

        public bool MemberEmailIsUnique(string email) {
            IEnumerable<Member> members = context.Members.Where(m => m.Email.Equals(email));
            return members.Count() == 0;            
        }

        public static bool HasTemporaryPassword(string email) {
            int memberKey = GetMemberKey(email);
            if (memberKey > 0) {
                PasswordType pType = (PasswordType)GetMembership(memberKey).PasswordType;
                return (pType == PasswordType.Temporary);
            } else {
                throw new ApplicationException("User Not Found");
            }
        }

        public static bool ChangePassword(string oldPassword, string newPassword, int userKey) {
            bool changeSucessful = false;
            Membership membership = null;
            try {
                membership = GetMembership(userKey);
                if (membership != null) {
                    Authentication authentication = new Authentication();
                    if (authentication.DoesPasswordMatch(oldPassword, membership.Password, membership.Salt)) {
                        membership.Password = authentication.EncryptPassword(newPassword);
                        membership.Salt = authentication.Salt;
                        membership.PasswordType = (int)PasswordType.Valid;
                        membership.LastPasswordChangeDate = DateTime.Now;
                        context.SaveChanges();
                        changeSucessful = true;
                    }
                }
            }
            catch (Exception) {
                throw;
            }
            return changeSucessful;
        }

        public static bool ChangeEmail(string password, string newEmail, int userKey) {
            bool changeSucessful = false;
            Membership membership = null;
            try {
                membership = GetMembership(userKey);
                if (membership != null) {
                    Authentication authentication = new Authentication();
                    if (authentication.DoesPasswordMatch(password, membership.Password, membership.Salt)) {
                        Member m = GetMember(userKey);
                        membership.LoweredEmail = newEmail.ToLower();
                        m.Email = newEmail;
                        context.SaveChanges();
                        changeSucessful = true;
                    }
                }
            }
            catch (Exception) {
                throw;
            }
            return changeSucessful;
        }


        public static bool ResetMemberPassword(int memberKey) {
            bool success = false;
            Membership membership = GetMembership(memberKey);
            if (membership.MemberKey > 0) {
                Member member = GetMember(memberKey);
                Authentication auth = new Authentication();
                string tempPassword = auth.TemporaryPassword;

                membership.Password = auth.EncryptPassword(tempPassword);
                membership.Salt = auth.Salt;
                membership.PasswordType = (short)PasswordType.Temporary;
                context.SaveChanges();
                SendTempPassword(tempPassword, member.Email, member.LastName);
                success = true;
            }
            return success;
        }

        public void LockMember(int memberKey) {
            Membership membership = GetMembership(memberKey);
            membership.IsLocked = true;
            context.SaveChanges();
        }

        public void UnLockMember(int memberKey) {
            Membership membership = GetMembership(memberKey);
            membership.IsLocked = false;
            context.SaveChanges();
        }

        public bool SendMessageToMembersFromMember(List<int> toMemberKeys, int fromMemberKey, string toName, string subject, string body) {
            bool messageSent = false;
            List<string> memberEmails = new List<string>();
            Member toMember = null;
            Member fromMember = null;
            bool allMembersFound = true;
            try {
                foreach (int key in toMemberKeys) {
                    toMember = GetMember(key);
                    if (toMember != null) {
                        memberEmails.Add(toMember.Email);
                    } else {
                        allMembersFound = false;
                        break;
                    }
                }
                fromMember = GetMember(fromMemberKey);

                if (allMembersFound == true && fromMember != null) {
                    SendEmailNotice(memberEmails, toName, fromMember.Email, subject, body);
                    messageSent = true;
                }
            } catch (Exception) {
                messageSent = false;
            }
            return messageSent;
        }

        public int CreateAddress(string streetAddress1, string streetAddress2, string city, string state, string zip, decimal latitude = 0, decimal longitude = 0) {
            Address address = new Address() {
                StreetAddress1 = streetAddress1,
                StreetAddress2 = streetAddress2,
                City = city,
                State = state,
                ZipCode = zip,
                Latitude = latitude,
                Longitude = longitude
            };
            context.Addresses.AddObject(address);
            context.SaveChanges();

            return address.AddressKey;

        }

        public bool UpdateAddress(int addressKey, string streetAddress1, string streetAddress2, string city, string state, string zip, decimal latitude = 0, decimal longitude = 0) {
            bool updated = false;

            Address address = context.Addresses.Where(a => a.AddressKey == addressKey).Single();
            address.StreetAddress1 = streetAddress1;
            address.StreetAddress2 = streetAddress2;
            address.City = city;
            address.State = state;
            address.ZipCode = zip;
            address.Latitude = latitude;
            address.Longitude = longitude;
            context.SaveChanges();
            updated = true;

            return updated;
        }

        public Address GetAddress(int addressKey) {
            return context.Addresses.Where(a => a.AddressKey == addressKey).FirstOrDefault();
        }

        //public void CreateMemberContact(int memberKey, string firstName, string middleName, string lastName, string phone, string amaNumber, DateTime dob, int addressKey) {
        //    MemberContact contact = new MemberContact() {
        //        FirstName = firstName,
        //        MiddleName = middleName,
        //        LastName = lastName,
        //        Phone = phone,
        //        AMANumber = amaNumber,
        //        DateOfBirth = dob,
        //        AddressKey = addressKey, 
        //        MemberKey = memberKey
        //    };
        //    context.MemberContacts.AddObject(contact);
        //    context.SaveChanges();
        //}

        public void UpdateMember(int memberKey, string firstName, string middleName, string lastName, string phone, string amaNumber, DateTime dob) {

            Member contact = context.Members.Where(m => m.MemberKey == memberKey).Single();
            contact.FirstName = firstName;
            contact.MiddleName = middleName;
            contact.LastName = lastName;
            contact.Phone = phone;
            contact.LicenseNumber = amaNumber;
            contact.DateOfBirth = dob;

            context.SaveChanges();

        }

        //public MemberContact GetMemberContact(int memberKey) {
        //    MemberContact contact = context.MemberContacts.Where(m => m.MemberKey == memberKey).FirstOrDefault();
        //    return contact;
        //}


        public void CreateMotorcycle(int memberKey, string brand, string model, short year, string displacement, string ridingNumber) {

            using (TransactionScope scope = new TransactionScope()) {
                Motorcycle motorcycle = new Motorcycle() {
                    Brand = brand,
                    Model = model,
                    Year = year,
                    Displacement = displacement,
                    RidingNumber = ridingNumber
                };

                context.Motorcycles.AddObject(motorcycle);
                context.SaveChanges();

                MemberMotorcycle memberMotorcycle = new MemberMotorcycle() {
                    MemberKey = memberKey,
                    MotorcycleKey = motorcycle.MotorcycleKey
                };

                context.MemberMotorcycles.AddObject(memberMotorcycle);
                context.SaveChanges();
                scope.Complete();
            }
           

        }

        public void UpdateMotorcycle(int motorcycleKey, string brand, string model, short year, string displacement, string ridingNumber) {
            Motorcycle motorcycle = context.Motorcycles.Where(m => m.MotorcycleKey == motorcycleKey).Single();
            motorcycle.Brand = brand;
            motorcycle.Model = model;
            motorcycle.Year = year;
            motorcycle.Displacement = displacement;
            motorcycle.RidingNumber = ridingNumber;
            context.SaveChanges();
        }


        public bool DeleteMotorcycle(int memberKey, int motorcycleKey) {
            bool deleted = false;
            bool canDelete = context.MemberMotorcycles.Any(mm => mm.MemberKey == memberKey && mm.MotorcycleKey == motorcycleKey);
            if (canDelete) {
                using (TransactionScope scope = new TransactionScope()) {
                    MemberMotorcycle memberMotorcycle = context.MemberMotorcycles.Where(mm => mm.MemberKey == memberKey && mm.MotorcycleKey == motorcycleKey).Single();
                    context.DeleteObject(memberMotorcycle);
                    context.SaveChanges();

                    Motorcycle motorcycle = context.Motorcycles.Where(m => m.MotorcycleKey == motorcycleKey).Single();
                    context.Motorcycles.DeleteObject(motorcycle);
                    context.SaveChanges();
                    scope.Complete();
                    deleted = true;
                }
            }
            return deleted;
        }

        public Motorcycle GetMemberMotorcycle(int motorcycleKey) {
            Motorcycle motorcycle = context.Motorcycles.Where(m => m.MotorcycleKey == motorcycleKey).Single();
            return motorcycle;
        }


        public IEnumerable<Motorcycle> GetMemberMotorcycles(int memberKey) {
            IEnumerable<Motorcycle> motorcycles = from m in context.Motorcycles 
                                                  join mm in context.MemberMotorcycles 
                                                  on m.MotorcycleKey equals mm.MotorcycleKey 
                                                  where mm.MemberKey == memberKey 
                                                  select m;
            return motorcycles;
        }

        //public IEnumerable<MemberContactView> GetMemberContactViews() {
        //    IEnumerable<MemberContactView> memberContactViews = context.MemberContactViews.Select(m => m);
        //    return memberContactViews;
        //}

        //public MemberContactView GetMemberContactView(int memberKey) {
        //    MemberContactView memberContactView = context.MemberContactViews.Where(m => m.MemberKey == memberKey).Single();
        //    return memberContactView;
        //}

        //public int GetMemberKey(int memberKey) {
        //    int memberContactKey = 0;
        //    MemberContact contact = context.MemberContacts.Where(m => m.MemberKey == memberKey).Single();
        //    if (contact != null) {
        //        memberContactKey = contact.MemberContactKey;
        //    }
        //    return memberContactKey;
        //}       

        private static void SendTempPassword(string tempPassword, string emailAddress, string userName) {
            //string fromEmail = @"postmaster@essgolfhc.com";
            string fromEmail = @"lukeedlund@comcast.net";           
            
            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromEmail);
            message.To.Add(emailAddress);
            message.IsBodyHtml = false;
            message.Subject = "Welcome to Masters MX ";
            message.Body = "Hello " + userName + ", your temporary password is: " + tempPassword;
            message.Body += "\nPlease go to http://essgolfhc.com You will be directed to change this password after login.";
            message.Body += "\n Thank you for using the Masters MX application";

            SendMail(message);

        }

    

        private static void SendEmailNotice(List<string> emailAddresses, string toName, string fromEmail, string subject, string body) {
            
            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromEmail);
            foreach (string s in emailAddresses) {
                message.To.Add(s);
            }
            message.Subject = subject;
            message.Body = "Hello " + toName + ",\n"  + body;
            message.Body += "\n Thank you for using ESS Golf HC";
            message.Body += "\n\n This message was sent from http://essgolfhc.com ";
            SendMail(message);

        }


        private static void SendMail(MailMessage message) {
            //SmtpClient smtp = new SmtpClient("localhost");
            SmtpClient smtp = new SmtpClient("mail.comcast.net");
            try {
                smtp.Send(message);
            }
            catch (Exception e) {
                throw e;
            }
        }
        public IEnumerable<Member> GetMembers(short year) {            
            IEnumerable<Member> members = (from mm in context.Members 
                                          join rcm in context.RaceClassMembers on mm.MemberKey equals rcm.MemberKey
                                          where rcm.Year == year select mm).Distinct().OrderBy(mm => mm.LastName);
            return members;
        }

    }
}
