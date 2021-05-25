using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMXServiceInterfaceLib;
using MMXDataLib;
using MMXModelsLib;
using System.Transactions;



namespace MMXDataService {
    public class MemberService : IMemberService {
        private readonly MemberDataController _controller;
        private readonly RaceClassDataController _raceClassController;
        private readonly RaceClassMemberService _raceClassMemberService;

        public MemberService() {
            _controller = new MemberDataController();
            _raceClassController = new RaceClassDataController();
            _raceClassMemberService = new RaceClassMemberService();
        }


        public MemberModel GetMemberSummary(int memberKey) {
            MemberSummary summary = _controller.GetMemberSummary(memberKey);
            return CreateMemberModel(summary, memberKey);
        }

        public List<MemberModel> GetMemberSummaries() {
            List<MemberModel> memberModels = new List<MemberModel>();
            IEnumerable<MemberSummary> summaries = _controller.GetMemberSummaries();
            foreach (MemberSummary s in summaries) {
                memberModels.Add(CreateMemberModel(s, s.MemberKey));
            }
            return memberModels;
        }

        private MemberModel CreateMemberModel(MemberSummary summary, int memberKey) {
            MemberModel model = new MemberModel();
            bool IsAdmin = false;
            if (memberKey > 0) {
                Role role = _controller.GetMemberRole(memberKey);
                IsAdmin = role.RoleKey < 3;
            }
            model.Email = summary.Email;
            model.LastActivity = summary.LastActivityDate;
            model.LastLoginDate = summary.LastLoginDate;
            model.LastPasswordChangeDate = summary.LastPasswordChangeDate;
            model.MemberKey = summary.MemberKey;
           // model.UserName = summary.;
            model.IsAdminRole = IsAdmin;
            model.IsLocked = summary.IsLocked;

            return model;
        }


        public bool IsAdministrator(int memberKey) {
            Role role = _controller.GetMemberRole(memberKey);
            return role.RoleName.Contains("Admin");
        }

        public bool IsSuperUser(int memberKey) {
            Role role = _controller.GetMemberRole(memberKey);
            return role.RoleName.Contains("Super");
        }

        public bool IsValidUser(int memberKey) {
            bool isValid = false;
            Membership membership = MemberDataController.GetMembership(memberKey);
            if (membership != null) {

                isValid = ((MMXDataLib.MemberDataController.PasswordType)membership.PasswordType) 
                    == MMXDataLib.MemberDataController.PasswordType.Valid;
            }
            return isValid;
        }

        //public bool SendMessageToGroupMembers(List<int> toMemberKeys, int fromMemberKey, string groupName, string subject, string body) {
        //    return _controller.SendMessageToMembersFromMember(toMemberKeys, fromMemberKey, groupName, subject, body);
        //}

        //public MemberMessageModel GetGroupMessageModel(int groupKey) {
        //    MemberMessageModel model = new MemberMessageModel();
        //    ScoringGroup group = _controller.GetScoringGroup(groupKey);
        //    if (group != null) {
        //        model.GroupName = group.GroupName;
        //        model.Subject = "A message for " + group.GroupName;
        //        model.Body = "Scores Entered / Updated";
        //    }
        //    model.GroupMembers = this.GetGroupMembers(groupKey, true);

        //    return model;

        //}


        //public List<string> GetMemberEmailList(List<int> memberKeys) {
        //    List<string> emailAddresses = new List<string>();
        //    foreach (int key in memberKeys) {
        //       // Member member = MemberDataController.GetMember(key);
        //        if (member != null) {
        //            emailAddresses.Add(member.Email);
        //        }
        //    }
        //    return emailAddresses;
        //}      
        



        public int CreateAddress(AddressModel addressModel) {
            return _controller.CreateAddress(addressModel.StreetAddress1, addressModel.StreetAddress2, 
                addressModel.City, addressModel.State, addressModel.ZipCode, addressModel.Latitude, 
                addressModel.Longitude);
        }

        public bool UpdateAddress(AddressModel addressModel) {
            return _controller.UpdateAddress(addressModel.AddressKey, addressModel.StreetAddress1, addressModel.StreetAddress2,
                addressModel.City, addressModel.State, addressModel.ZipCode, addressModel.Latitude,
                addressModel.Longitude);
        }

        public void CreateMember(MemberModel member) {
            using (TransactionScope scope = new TransactionScope()) {
                int addressKey = _controller.CreateAddress(member.Address.StreetAddress1,
                    member.Address.StreetAddress2, member.Address.City,
                    member.Address.State, member.Address.ZipCode,
                    member.Address.Latitude, member.Address.Longitude);
                _controller.CreateMember(member.Email, member.FirstName,
                    member.MiddleName, member.LastName, member.Suffix, member.Phone,
                    member.LicenseNumber, member.DateOfBirth, addressKey);

                scope.Complete();
            }
        }

        public void UpdateMember(MemberModel member) {
            _controller.UpdateAddress(member.AddressKey, member.Address.StreetAddress1,
                    member.Address.StreetAddress2, member.Address.City,
                    member.Address.State, member.Address.ZipCode,
                    member.Address.Latitude, member.Address.Longitude);
            _controller.UpdateMember(member.MemberKey, member.FirstName,
                member.MiddleName, member.LastName, member.Phone,
                member.LicenseNumber, member.DateOfBirth);
        }

        public void CreateMotorcycle(MotorcycleModel motorcycleModel) {
            _controller.CreateMotorcycle(motorcycleModel.MemberKey, motorcycleModel.Brand, motorcycleModel.CycleModel,
                motorcycleModel.Year, motorcycleModel.Displacement, motorcycleModel.RidingNumber);
        }

        public void UpdateMotorcycle(MotorcycleModel motorcycleModel) {
            _controller.UpdateMotorcycle(motorcycleModel.MotorcycleKey, motorcycleModel.Brand, motorcycleModel.CycleModel,
                motorcycleModel.Year, motorcycleModel.Displacement, motorcycleModel.RidingNumber);
        }

        public bool DeleteMotorcycle(int memberContactKey, int motorcycleKey) {
            return _controller.DeleteMotorcycle(memberContactKey, motorcycleKey);
        }

        public MemberModel GetMember(int memberKey) {
            MemberModel model = null;
            Member contact = MemberDataController.GetMember(memberKey);
            if (contact != null) {
                model = new MemberModel();
                if (contact.AddressKey != null) {
                    model.Address = GetAddress((int)contact.AddressKey);
                    model.AddressKey = (int)contact.AddressKey;
                }
                //model.Member = GetMemberSummary(memberKey);               
                
                if (contact.DateOfBirth != null) {
                    model.DateOfBirth = (DateTime)contact.DateOfBirth;
                }
                model.FirstName = contact.FirstName;
                model.MiddleName = contact.MiddleName;
                model.LastName = contact.LastName;
                model.Suffix = contact.Suffix;
                model.MemberKey = memberKey;
                model.Phone = contact.Phone;
                model.Email = contact.Email;
                model.LicenseNumber = contact.LicenseNumber;

            }
            return model;
        }

        public AddressModel GetAddress(int addressKey) {
            AddressModel model = null;

            Address address = _controller.GetAddress(addressKey);
            if (address != null) {
                model = new AddressModel() {
                    AddressKey = address.AddressKey,
                    City = address.City,
                    Latitude = address.Latitude == null ? 0 : (decimal) address.Latitude,
                    Longitude = address.Longitude == null ? 0 : (decimal) address.Longitude,
                    State = address.State,
                    StreetAddress1 = address.StreetAddress1,
                    StreetAddress2 = address.StreetAddress2,
                    ZipCode = address.ZipCode
                };

            }

            return model;
        }

        //public int GetMemberContactKey(int memberKey) {
        //    return _controller.GetMemberContactKey(memberKey);
        //}


        public IEnumerable<MotorcycleModel> GetMemberMotorcycles(int memberKey) {
            IEnumerable<Motorcycle> motorcycles = _controller.GetMemberMotorcycles(memberKey);
            List<MotorcycleModel> models = new List<MotorcycleModel>();
            foreach (Motorcycle m in motorcycles) {
                MotorcycleModel model = new MotorcycleModel() {
                    Brand = m.Brand,
                    CycleModel = m.Model,
                    Displacement = m.Displacement,
                    MemberKey = memberKey,
                    MotorcycleKey = m.MotorcycleKey,
                    RidingNumber = m.RidingNumber,
                    Year = (short) m.Year
                };
                models.Add(model);
            }

            return models;
        }


        public MotorcycleModel GetMemberMotorcycle(int motorcycleKey, int memberKey) {            
            Motorcycle m = _controller.GetMemberMotorcycle(motorcycleKey);
            MotorcycleModel model = new MotorcycleModel() {
                Brand = m.Brand,
                CycleModel = m.Model,
                Displacement = m.Displacement,
                MemberKey = memberKey,
                MotorcycleKey = m.MotorcycleKey,
                RidingNumber = m.RidingNumber,
                Year = (short)m.Year
            };

            return model;
        }


        public RoleModel GetMemberRole(int memberKey) {
            Role role = _controller.GetMemberRole(memberKey);
            RoleModel model = new RoleModel() {
                RoleKey = role.RoleKey,
                RoleName = role.RoleName
            };
            return model;
        }


        public List<MemberDisplayModel> GetMembers(short year) {
            if (year == 0) {
              year = _raceClassMemberService.GetCurrentEnrollmentYear();
            }

            List<MemberDisplayModel> models = new List<MemberDisplayModel>();
            IEnumerable<Member> members = _controller.GetMembers(year);
            foreach (Member member in members) {
                MemberDisplayModel mdm = new MemberDisplayModel() {
                    MemberKey = member.MemberKey,
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    MiddleName = member.MiddleName,
                    Suffix = member.Suffix
                };

                IEnumerable<RaceClass> raceClasses = _raceClassController.GetCurrentRaceClassesForMember(mdm.MemberKey, year);
                StringBuilder sb = new StringBuilder();
                string classFormat = "{0} "; 
                foreach (RaceClass rc in raceClasses) {
                    sb.AppendFormat(classFormat, rc.ClassName);
                }
                mdm.Classes = sb.ToString();
                models.Add(mdm);

            }          

            return models;
        }
    }
}
