using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMXServiceInterfaceLib;
using System.Web.Routing;
using MMXDataService;
using MMXModelsLib;
using System.Text;

namespace MMXApplication.Controllers
{
    public class RaceController : BaseController  {

        private IRaceClassMemberService raceClassMemberService { get; set; }
        private IRaceService raceService { get; set; }

        protected override void Initialize(RequestContext requestContext) {
            base.Initialize(requestContext);
            if (raceClassMemberService == null) {
                raceClassMemberService = new RaceClassMemberService();
            }
            if (raceService == null) {
                raceService = new RaceService();
            } 

        }

        // todo update or remove
        public ActionResult RaceClassList() {
            SetTabClasses(classesTab);
            
            RaceClassModels rcm = raceClassMemberService.GetRaceClassesForMember(MemberKey);
            rcm.IsAdminRole = IsAdminRole;
            rcm.ShowAll = false;
            return BaseView("RaceClassList", rcm, "My Race Classes");
        }

        public ActionResult ShowAllRaceClasses() {
            SetTabClasses(classesTab);            
            RaceClassModels rcm = raceClassMemberService.GetAllRaceClasses();
            rcm.IsAdminRole = IsAdminRole;
            rcm.ShowAll = true;
            return BaseView("RaceClassList", rcm, "All Race Classes");
        }

        public ActionResult ShowActiveRaceClasses() {
            SetTabClasses(classesTab);
            
            RaceClassDisplayListModel rcListModel = new RaceClassDisplayListModel();
            IEnumerable<RaceClassDisplayModel> rcm = raceClassMemberService.GetActiveRaceClasses();
            rcListModel.IsAdminRole = IsAdminRole;
            ViewData.Add("IsAdmin", IsAdminRole);
            rcListModel.Year = 2013; // todo raceClassMemberService.GetCurrentEnrollmentYear();
            rcListModel.RaceClassDisplayModels = rcm.ToList();
            return BaseView("RaceClassList", rcListModel, "Active Race Classes");
        }       

        [Authorize]
        public ActionResult AddUpdateRaceClass(int raceClassKey /*, bool showingAll*/) {
            SetTabClasses(classesTab);
            RaceClassModel model = null;
            if (raceClassKey < 1) {
                model = new RaceClassModel();                
            } else {
                model = raceClassMemberService.GetRaceClass(raceClassKey);
            }
            //model.ShowingAll = showingAll;
            return BaseView("AddUpdateRaceClass", model, "Add Update Race Class");
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddUpdateRaceClass(RaceClassModel model) {
            SetTabClasses(classesTab);
            if (model.RaceClassKey < 1) {
                raceClassMemberService.CreateRaceClass(model);
            } else {
                raceClassMemberService.UpdateRaceClass(model);
            }
            return ShowActiveRaceClasses();
        }

        
        public ActionResult ShowRaceClassMembers(int raceClassKey, bool showingAll) {
            // todo select current years members            
            short currentYear = raceClassMemberService.GetCurrentEnrollmentYear();
            ViewData.Add("IsAdmin", IsAdminRole);
            SetTabClasses(classesTab);
            RaceClassModel model = raceClassMemberService.GetRaceClass(raceClassKey);
            model.ShowingAll = showingAll;

            IEnumerable<RaceClassMemberModel> members = raceClassMemberService.GetRaceClassMemberViews(raceClassKey, currentYear);
            model.Members = members.ToList();
            return PartialView("RaceClassMembers", model);

        }

        public ActionResult GetRaceClassMembers(int raceClassKey) {
            short currentYear = 2013; // todo raceClassMemberService.GetCurrentEnrollmentYear();
            ViewData.Add("IsAdmin", IsAdminRole);
            IEnumerable<RaceClassMemberModel> members = raceClassMemberService.GetRaceClassMemberViews(raceClassKey, currentYear);
            return PartialView("RaceClassMemberList", members);

        }

        [Authorize]
        public ActionResult RaceClassSignUp(int raceClassKey) {
            SetTabClasses(classesTab);
            MemberService service = new MemberService();
            RaceClassSignUpModel signUpModel = new RaceClassSignUpModel();
            signUpModel.Motorcycles = service.GetMemberMotorcycles(MemberKey).ToList();
            RaceClassModel rcModel = raceClassMemberService.GetRaceClass(raceClassKey);
            signUpModel.SelectedRaceClassModel = rcModel;

            return BaseView("RaceClassSignUp", signUpModel, "Class Sign Up");
        }

        [Authorize]
        [HttpPost]
        public ActionResult RaceClassSignUp(RaceClassSignUpModel signUpModel) {
            StringBuilder sb = new StringBuilder();
            bool sucess = raceClassMemberService.CreateRaceClassMember(signUpModel.RaceClassKey, MemberKey, signUpModel.MotorcycleKey, false, sb);
            if (!sucess) {
                SetTabClasses(classesTab);
                MemberService service = new MemberService();
                ViewData.Add("SignUpError", sb.ToString());
                RaceClassModel rcModel = raceClassMemberService.GetRaceClass(signUpModel.RaceClassKey);
                signUpModel.SelectedRaceClassModel = rcModel;
                signUpModel.Motorcycles = service.GetMemberMotorcycles(MemberKey).ToList();
                return BaseView("RaceClassSignUp", signUpModel, "Class Sign Up");
            } else {
                return RaceClassList();
            }
        }

       
        public ActionResult RaceSchedule(short year = 0) {
            SetTabClasses(scheduleTab);
            ViewData.Add("IsAdmin", IsAdminRole);
            RaceScheduleModel model = new RaceScheduleModel();
            short enrollmentYear = raceClassMemberService.GetCurrentEnrollmentYear();
            model.Year = year > 0 ? year : enrollmentYear;
            model.Races = raceService.GetYearsRaces(model.Year);
            model.Tracks = raceService.GetTracks();
            model.YearSelector = new YearSelectorModel();
            model.YearSelector.Years = RaceResultController.GetYears(model.Year, enrollmentYear);
            
           
            return BaseView("RaceSchedule", model, "Race Schedule");
        }

        [HttpPost]
        public ActionResult RaceSchedule(YearSelectorModel model) {
            if (model != null) {
                return RaceSchedule(model.CurrentYear);
            } else {
                return RaceSchedule();
            }
        }

        [Authorize]
        public ActionResult AddUpdateTrack(int trackKey) {
            SetTabClasses(scheduleTab);
            TrackModel model = null;
            if (trackKey > 0) {
                model = raceService.GetTrack(trackKey);
               
            } else {
                model = new TrackModel();
                model.Address = new AddressModel();
            }
            return BaseView("AddUpdateTrack", model, "Add Update Track");            

        }

        [Authorize]
        [HttpPost]
        public ActionResult AddUpdateTrack(TrackModel model) {            
            if (model.TrackKey > 0) {
                raceService.UpdateTrack(model);
            } else {
                raceService.CreateTrack(model);
            }
            return RaceSchedule();
        }

        
        public ActionResult GetTrackDetails(int trackKey) {
            TrackModel model = raceService.GetTrack(trackKey);
            return PartialView("TrackDetail", model);
        }

        [Authorize]
        public ActionResult AddUpdateRace(int raceKey) {
            SetTabClasses(scheduleTab);
            RaceModel model = null;
            if (raceKey < 1) {
                model = new RaceModel();
            } else {
                model = raceService.GetRace(raceKey);

            }
            model.Tracks = raceService.GetTracks();

            return BaseView("AddUpdateRace", model, "Race");

        }

        [Authorize]
        [HttpPost]
        public ActionResult AddUpdateRace(RaceModel model) {
            if (model.RaceKey < 1) {
                raceService.CreateRace(model);
            } else {
                raceService.UpdateRace(model);
            }
            return RaceSchedule();
        }
     
    }
}
