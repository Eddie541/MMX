using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMXServiceInterfaceLib;
using MMXDataLib;
using MMXModelsLib;

namespace MMXDataService {
    public class RaceClassMemberService : IRaceClassMemberService {

        private readonly RaceClassDataController _controller;

        public RaceClassMemberService() {
            _controller = new RaceClassDataController();
        }

        public void CreateRaceClass(RaceClassModel raceClassModel) {
            _controller.CreateRaceClass(raceClassModel.ClassName, raceClassModel.MinimumAge, raceClassModel.Note);
        }

        public void UpdateRaceClass(RaceClassModel raceClassModel) {
            _controller.UpdateRaceClass(raceClassModel.RaceClassKey, raceClassModel.ClassName, raceClassModel.MinimumAge, raceClassModel.Note);
        }

        public void DisableRaceClass(int raceClassKey) {
            _controller.DisableRaceClass(raceClassKey);
        }

        public bool OpenEnrollmentForNextYear() {
           return _controller.OpenEnrollmentForNextYear();
        }

        public bool CloseEnrollmentForCurrentYear() {
            return _controller.CloseEnrollmentForCurrentYear();
        }

        public void ResetEnrollmentYear(short year, bool isOpen) {
            _controller.ResetEnrollmentYear(year, isOpen);
        }

        public short GetCurrentEnrollmentYear() {
            return _controller.CurrentEnrollmentYear;
        }

        public EnrollmentModel GetEnrollment() {
            Enrollment enrollment = _controller.GetEnrollment();
            EnrollmentModel model = new EnrollmentModel() {
                DropPoints = enrollment.DropPoints,
                EnrollmentOpen = enrollment.EnrollmentOpen,
                EnrollmentYear = enrollment.Year

            };
            return model;
        }

        public bool CreateRaceClassMember(int raceClassKey, int contactKey, int memberMotorcycleKey, bool hasPaid, StringBuilder sb) {                       
            return _controller.CreateRaceClassMember(raceClassKey, contactKey, memberMotorcycleKey, hasPaid, sb);
        }

        //public bool CreateImportedRaceClassMember(string raceClassName, int memberKey, short year) {
        //    return _controller.CreateImportedRaceClassMember(raceClassName, memberKey, year);
        //}

        public bool IsNumberUsedInClass(int raceClassKey, string ridingNumber) {
            return _controller.IsNumberUsedInClass(raceClassKey, ridingNumber);
        }

        public void UpdateRaceClassMemberRide(int raceClassMemberKey, int newMotorcycleKey) {
            throw new NotImplementedException();
            //_controller.UpdateRaceClassMemberRide(raceClassMemberKey, newMotorcycleKey);
        }

        public void UpdateRaceClassMemberHasPaid(int raceClassMemberKey, bool hasPaid) {
            _controller.UpdateRaceClassMemberHasPaid(raceClassMemberKey, hasPaid);
        }

        public string DeleteRaceClassMember(int raceClassMemberKey) {
            return _controller.DeleteRaceClassMember(raceClassMemberKey);
        }      

        public short GetRaceMemberAge(int memberContactKey) {
            return _controller.GetRaceMemberAge(memberContactKey);
        }

        public IEnumerable<RaceClassMemberModel> GetRaceClassMemberViews(int raceClassKey, short year) {
            List<RaceClassMemberModel> models = new List<RaceClassMemberModel>();
            IEnumerable<RaceClassMemberView> views = _controller.GetRaceClassMemberViews(raceClassKey, year);

            foreach (RaceClassMemberView view in views) {

                short memberAge = _controller.GetRaceMemberAge(view.MemberKey);
                RaceClassMemberModel model = new RaceClassMemberModel() {
                    Age = memberAge,
                    FirstName = view.FirstName,
                    MiddleName = view.MiddleName,
                    LastName = view.LastName,
                    City = view.City,
                    State = view.State,
                    Year = view.Year,                    
                    RaceClassKey = view.RaceClassKey,
                    RaceClassMemberKey = view.RaceClassMemberKey,
                    HasPaid = view.HasPaid,
                    PointsTotal = (short)(view.PointsTotal == null ? 0 : view.PointsTotal),
                    PointsDropped = (short)(view.PointsDropped == null ? 0 : view.PointsDropped),                    
                    AdjustedPointsTotal = (short)(view.AdjustedPointTotal == null ? 0 : view.AdjustedPointTotal)                    
                };

                models.Add(model);
            }

            return models;
        }


        public RaceClassMemberModel GetRaceClassMemberView(int raceClassMemberKey) {
            RaceClassMemberModel model = null;
            RaceClassMemberView view = _controller.GetRaceClassMemberView(raceClassMemberKey);
            if (view != null) {
                short memberAge = _controller.GetRaceMemberAge(view.MemberKey);
                model = new RaceClassMemberModel() {
                    Age = memberAge,
                    FirstName = view.FirstName,
                    MiddleName = view.MiddleName,
                    LastName = view.LastName,
                    RaceClassKey = view.RaceClassKey,
                    RaceClassMemberKey = view.RaceClassMemberKey,
                    HasPaid = view.HasPaid

                };

            }

            return model;
        }


        public RaceClassModels GetRaceClassesForMember(int memberContactKey) {
            RaceClassModels rcModels = new RaceClassModels();
            IEnumerable<RaceClass> raceClasses = _controller.GetMemberRaceClasses(memberContactKey);            
            foreach (RaceClass rc in raceClasses) {
                RaceClassModel model = new RaceClassModel() {
                    ClassName = rc.ClassName,
                    Enabled = rc.Enabled,
                    MinimumAge = (short)rc.MinumumAge,
                    Note = rc.Note,
                    RaceClassKey = rc.RaceClassKey

                };
                rcModels.Models.Add(model);
                

            }

            return rcModels;
        }

        public RaceClassModels GetAllRaceClasses() {
            IEnumerable<RaceClass> raceClasses = _controller.GetAllRaceClasses().ToList();
            RaceClassModels  rcModels = new RaceClassModels();            
            foreach (RaceClass rc in raceClasses) {
                RaceClassModel model = new RaceClassModel() {
                    ClassName = rc.ClassName,
                    Enabled = rc.Enabled,
                    MinimumAge = (short)rc.MinumumAge,
                    Note = rc.Note,
                    RaceClassKey = rc.RaceClassKey

                };
                rcModels.Models.Add(model);

            }                      
            return rcModels;
        }

        public IEnumerable<RaceClassDisplayModel> GetActiveRaceClasses() {
            IEnumerable<RaceClass> raceClasses = _controller.GetAllActiveRaceClasses().ToList();
            List<RaceClassDisplayModel> rcDisplayModels = new List<RaceClassDisplayModel>();
            foreach (RaceClass rc in raceClasses) {
                RaceClassDisplayModel model = new RaceClassDisplayModel() {
                    ClassName = rc.ClassName,
                    Enabled = rc.Enabled,
                    Note = rc.Note,
                    RaceClassKey = rc.RaceClassKey
                };
                rcDisplayModels.Add(model);

            }
            return rcDisplayModels;

        }


        public RaceClassModel GetRaceClass(int raceClassKey) {
            RaceClass raceClass = _controller.GetRaceClass(raceClassKey);
            RaceClassModel model = new RaceClassModel() {
                ClassName = raceClass.ClassName,
                Enabled = raceClass.Enabled,
                MinimumAge = (short)raceClass.MinumumAge,
                Note = raceClass.Note,
                RaceClassKey = raceClass.RaceClassKey

            };
            return model;
        }


       
    }
}
