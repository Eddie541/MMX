using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMXServiceInterfaceLib;
using MMXDataService;
using MMXModelsLib;
using System.Text;
using System.Web.UI;
using System.IO;

namespace MMXApplication.Controllers
{
    public class RaceResultController : BaseController {

        private IRaceService raceService;
        private IRaceClassMemberService raceClassMemberService;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext) {
            base.Initialize(requestContext);
            if (raceService == null) {
                raceService = new RaceService();
            }
            if (raceClassMemberService == null) {
                raceClassMemberService = new RaceClassMemberService(); 
            }
        }

        
        [HttpPost]
        public ActionResult RaceResultsList(YearSelectorModel model) {
            if (model != null) {
                return RaceResultsList(model.CurrentYear);
            } else {
                return RaceResultsList();
            }
        }
        
        
        public ActionResult RaceResultsList(short year = 0) {
            SetTabClasses(resultsTab);
            RaceResultViewModel raceResultViewModel = new RaceResultViewModel();
            short resultYear = raceClassMemberService.GetCurrentEnrollmentYear();
            short enrollmentYear = resultYear;
            if (year > 0) {
                resultYear = year;
            }
            raceResultViewModel.Year = resultYear;
            raceResultViewModel.CompletedRaces = raceService.GetRacesWithResults(resultYear).ToList();
            //raceResultViewModel.RaceResults = new List<RaceResultModel>(); //raceService.GetAllRaceResults(resultYear);
            YearSelectorModel yearSelector = new YearSelectorModel();
            yearSelector.Years = GetYears(resultYear, enrollmentYear);
            raceResultViewModel.YearSelector = yearSelector;
            raceResultViewModel.IsAdminRole = base.IsAdminRole;
            //raceResultViewModel.RaceSelector = new RaceSelectorModel();
            //raceResultViewModel.RaceSelector.Races = raceService.GetYearsRaces(resultYear);
            return BaseView("RaceResultList", raceResultViewModel, "Race Results");
        }


        


        public ActionResult RaceClassDisplayList(int raceKey) {
            RaceClassDisplayListModel rcdlm = new RaceClassDisplayListModel(); 
            rcdlm.RaceClassDisplayModels  = raceService.GetRaceClassesForResults(raceKey).ToList();
            //rcdlm.RaceKey = raceKey;            
            return PartialView("RaceClassDisplayList", rcdlm);
        }

       
        //public ActionResult ShowClassRaceResult(int raceKey, int raceClassKey) {
        //    SetTabClasses(resultsTab);
        //    RaceResultModel model = raceService.GetClassRaceResult(raceKey, raceClassKey);            
        //    return BaseView("ClassRaceResult", model, "Class Results " + model.Track.TrackName);
        //}

        [Authorize]
        public ActionResult RaceResultEntry(int raceKey) {
            RaceResultEntryModel rreModel = new RaceResultEntryModel();
            rreModel.Race = raceService.GetRace(raceKey);
            rreModel.Classes = raceClassMemberService.GetAllRaceClasses().Models;
            rreModel.IsAdminRole = this.IsAdminRole;
            SetTabClasses(resultsTab);
            return BaseView("RaceResultEntry", rreModel, "Select Race Class");
        }


        [Authorize]
        [HttpPost]
        public ActionResult RaceResultEntry(RaceSelectorModel model) {
            if (model != null) {
                RaceResultEntryModel rreModel = new RaceResultEntryModel();
                rreModel.Race = raceService.GetRace(model.RaceKey);
                rreModel.Classes = raceClassMemberService.GetAllRaceClasses().Models;
                rreModel.IsAdminRole = this.IsAdminRole;
                SetTabClasses(resultsTab);
                return BaseView("RaceResultEntry", rreModel, "Select Race Class");
            } else {
                return RaceResultsList();
            }
        }

        [Authorize]
        public ActionResult AddUpdateRaceClassResults(int raceKey, int raceClassKey, int raceClassMemberKey = 0, int raceResultKey = 0, List<ErrorMessageModel> errors = null) {
            SetTabClasses(resultsTab);
            RaceClassResultEntryModel model = raceService.GetRaceResultEntryModel(raceKey, raceClassKey, raceResultKey);
            model.IsAdminRole = this.IsAdminRole;
            if (errors != null && errors.Count() > 0 &&  raceClassMemberKey > 0) {
                model.AddModelEntryErrors(raceClassMemberKey, errors);
            }
            return BaseView("AddUpdateRaceClassResults", model, "Add Update Results");

        }

        //[Authorize]
        //[HttpPost]
        //public ActionResult AddUpdateRaceClassResults(RaceClassResultEntryModel model) {            
        //    bool okEntries = true;
        //    // todo one at a time send via ajax update partial    
        //    int i = 0;
        //    foreach (MemberRaceResultEntryModel result in model.MemberResultEntries) {
        //        StringBuilder sb = new StringBuilder();
                
        //            if (ValidateEntry(result, sb)) {
        //                if (result.RaceResultKey < 1) {
        //                    result.RaceKey = model.Race.RaceKey;
        //                   // okEntries = raceService.CreateMemberRaceResult(result);
        //                } else {
        //                    // else update - result or details or both
        //                   // raceService.UpdateMemberRaceResultPosition(result.RaceResultKey, result.MotoOnePosition, result.StatusMotoOne, result.MotoTwoPosition, result.StatusMotoTwo, result.Overall);
        //                }
        //            } else {
        //                //model.MemberResultEntries[i].Error = sb.ToString();
        //                okEntries = false;
        //            }
                
        //        i++;
              
        //    }
        //    if (okEntries == false) {               
        //        return BaseView("AddUpdateRaceClassResults", model);
        //    } else {
        //        return RaceResultEntry(model.Race.RaceKey);
        //    }

        //}

        //[Authorize]
        //public ActionResult UpdateRaceClassResult(MemberRaceResultEntryModel model) {
        //    if (model.RaceResultKey > 0) {
        //        model.EntrySuccess = false;
        //        model.Errors.Clear();
        //    }
        //    return PartialView("MemberRaceResultEntryForm", model);
        //}


        [Authorize]
        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public ActionResult AddUpdateRaceClassResult(MemberRaceResultEntryModel model) {
            bool isValidState = ValidateModelState(model);
            
            if (Request.IsAjaxRequest()) {
                string t = "blah";

            }
            
            if (!isValidState) {
                model.EntrySuccess = false;    
                
            } else {
                model.Errors.Clear();
                model.EntrySuccess = true;
                // todo capture business / data errors               
                raceService.CreateMemberRaceResult(model);
                // save fill data values
                // todo get values and show display and update button  
               //return AddUpdateRaceClassResults(model.RaceKey, model.RaceClassKey);
            }
            // todo include year??
            //return AddUpdateRaceClassResults(model.RaceKey, model.RaceClassKey, model.RaceClassMemberKey, model.RaceResultKey, model.Errors);
            return PartialView("MemberRaceResultEntryForm", model);
        }

        public ActionResult GetRaceClassResults(int raceKey, int raceClassKey) {
            List<RaceClassMemberResultModel> raceClassResults = raceService.GetRaceResultsForRaceClass(raceKey, raceClassKey).ToList();
            
            return PartialView("RaceClassResultsList", raceClassResults);
        }       

        public static List<SelectorKey> GetYears(short selectedYear, short enrollmentYear) {
            List<SelectorKey> years = new List<SelectorKey>();
            short startYear = enrollmentYear;
            while (startYear > 2006) {
                if (startYear != selectedYear) {
                    SelectorKey key = new SelectorKey() {
                        CurrentYear = startYear
                    };
                    years.Add(key);                   
                }
                startYear--;
            }
            return years;

        }

        private string uploadPath = "~/App_Data/MMXData";


        [Authorize]
        public ActionResult UploadResultFile(UploadRaceResultsViewModel uploadRaceResultsViewModel) {
            // todo set current year and next race
            //UploadRaceResultsViewModel uploadRaceResultsViewModel = new UploadRaceResultsViewModel();
             uploadRaceResultsViewModel.SelectedRaceModel = GetUploadRaceResultsModel(2012, 9);

            return PartialView("UploadResultFile", uploadRaceResultsViewModel);
        }

        [HttpPost]
        [Authorize]
        public ActionResult UploadResultFile(HttpPostedFileBase uploadFile, string worksheetName, int raceKey, int year) {
            SetTabClasses(applicationTab);
            var filePath = string.Empty;
            if (uploadFile == null || uploadFile.ContentLength <= 0) {
                // todo some error handling here
                return View();
            }
            if (worksheetName != null && raceKey > 0) {


                filePath = Path.Combine(Server.MapPath(this.uploadPath), "Results", Path.GetFileName(uploadFile.FileName));
                // todo validate file type etc..
                uploadFile.SaveAs(filePath);
                if (raceService.ProcessRaceResultFile(filePath, raceKey, worksheetName, year)) {

                }
                

            }

            // todo check this = call upload via ajax form
            return RedirectToAction("SuperUserDashboard", new { year = year, raceKey = raceKey });

        }

        protected SelectedRaceUploadModel GetUploadRaceResultsModel(short year = 0, int selectedRaceKey = 0) {
            SelectedRaceUploadModel uploadRaceResultsViewModel = new SelectedRaceUploadModel();
            short resultYear = raceClassMemberService.GetCurrentEnrollmentYear();
            short enrollmentYear = resultYear;
            if (year > 0) {
                resultYear = year;
            }
            uploadRaceResultsViewModel.CurrentYear = resultYear;
            uploadRaceResultsViewModel.IsAdminRole = base.IsAdminRole;
            uploadRaceResultsViewModel.RaceKey = selectedRaceKey;
            if (selectedRaceKey > 0) {
                RaceModel rm = raceService.GetRace(selectedRaceKey);
                uploadRaceResultsViewModel.SelectedRace = rm.RaceName;
            }
            return uploadRaceResultsViewModel;

        }

        [Authorize]
        public ActionResult SetPointsDrop() {
            raceService.SetPointsDropForEnrollmentYear();
            // todo return updated SuperUserDashboardModel 
            return BaseView("SuperUserDashboard"); 
        }

        [Authorize]
        public ActionResult OpenEnrollment() {
            raceClassMemberService.OpenEnrollmentForNextYear();
            // todo return updated SuperUserDashboardModel 
            return BaseView("SuperUserDashboard"); 
        }

        [Authorize]
        public ActionResult CloseEnrollment() {
            raceClassMemberService.CloseEnrollmentForCurrentYear();
            // todo return updated SuperUserDashboardModel 
            return BaseView("SuperUserDashboard");
        }
        
        [Authorize]
        [HttpPost]
        public ActionResult SuperUserDashboard(short year = 0, int raceKey = 0) {
            SetTabClasses(applicationTab);
            SuperUserDashboardModel model = new SuperUserDashboardModel();
            if (year > 0) {
                model.SelectedYear = year;
            }
            //model.FileUpload = GetUploadRaceResultsModel(year, raceKey);
            model.Enrollment = raceClassMemberService.GetEnrollment();

            return BaseView("SuperUserDashboard", model);             

        }

        [Authorize]
        public ActionResult SuperUserDashboard() {
            SetTabClasses(applicationTab);
            SuperUserDashboardModel model = new SuperUserDashboardModel();           
           
            //model.FileUpload = GetUploadRaceResultsModel(0, 0);
            model.Enrollment = raceClassMemberService.GetEnrollment();

            return BaseView("SuperUserDashboard", model);

        }

        [Authorize]
        public ActionResult SelectRaceResultUpload(short year, int? raceKey) {
            SelectRaceResultUploadModel model = new SelectRaceResultUploadModel();
            int selectedRaceKey = raceKey == null ? 0 : (int)raceKey;
            if (year == 0 && selectedRaceKey == 0) {
                short currentYear = (short)DateTime.Now.Year;
                model.Years = GetYears(0, currentYear);
                model.Races = null;
                
            } else if (year > 0 && selectedRaceKey == 0) {
                model.Years = GetYears(0, year);
                model.SelectedRaceModel.CurrentYear = year;
                model.Races = raceService.GetRacesWithoutResults(year).ToList();

            } else {
                model.SelectedRaceModel.CurrentYear = year;
                model.SelectedRaceModel.RaceKey = selectedRaceKey;
                if (selectedRaceKey > 0) {
                    RaceModel rm = raceService.GetRace(selectedRaceKey);
                    model.SelectedRaceModel.SelectedRace = rm.RaceName;
                }
            }
            return PartialView("SelectRaceResultUpload", model);
        }

        public ActionResult SelectedRaceUpload(string worksheet, short year, int raceKey) {
            SelectedRaceUploadModel model = new SelectedRaceUploadModel() {
                CurrentYear = year,
                RaceKey = raceKey
            };
            if (raceKey > 0) {
                RaceModel rm = raceService.GetRace(raceKey);
                model.SelectedRace = rm.RaceName;
            }

            return PartialView("SelectedRaceUpload", model);
        }
       


    }
}
