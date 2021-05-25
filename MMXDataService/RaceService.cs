using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMXServiceInterfaceLib;
using MMXDataLib;
using MMXModelsLib;

namespace MMXDataService {
    public class RaceService : IRaceService {

        private readonly RaceDataController _controller;
        RaceClassDataController _classController;
      

        public RaceService() {
            _controller = new RaceDataController();
            _classController = new RaceClassDataController();
        }

        public int CreateTrack(TrackModel trackModel) {
            MemberDataController memberData = new MemberDataController();
            int addressKey = memberData.CreateAddress(trackModel.Address.StreetAddress1, trackModel.Address.StreetAddress2, trackModel.Address.City, trackModel.Address.State, trackModel.Address.ZipCode, trackModel.Address.Latitude, trackModel.Address.Longitude);
            return _controller.CreateTrack(trackModel.TrackName, trackModel.ContactName, trackModel.Phone, trackModel.EmailAddress, trackModel.WebAddress, addressKey);
        }

        public void UpdateTrack(TrackModel trackModel) {
            MemberDataController memberData = new MemberDataController();
            memberData.UpdateAddress(trackModel.AddressKey, trackModel.Address.StreetAddress1, trackModel.Address.StreetAddress2, trackModel.Address.City, trackModel.Address.State, trackModel.Address.ZipCode, trackModel.Address.Latitude, trackModel.Address.Longitude);
            _controller.UpdateTrack(trackModel.TrackKey, trackModel.TrackName, trackModel.ContactName, trackModel.Phone, trackModel.EmailAddress, trackModel.WebAddress);
        }

        public void CloseTrack(int trackKey) {
            _controller.CloseTrack(trackKey);
        }

        public TrackModel GetTrack(int trackKey) {
            TrackModel model = null;
            AddressModel addressModel = null;
            Track track = _controller.GetTrack(trackKey);
            
            if (track != null) {
                addressModel =  GetAddressModel(track.AddressKey);         

                model = new TrackModel() {
                    TrackName = track.TrackName,
                    ContactName = track.ContactName,
                    EmailAddress = track.EmailAddress,
                    TrackKey = track.TrackKey,                   
                    AddressKey = track.AddressKey,                  
                    WebAddress = track.WebAddress,                  
                    IsOpen = track.IsOpen,
                    Address = addressModel
                };              
            }
            return model;
        }

        public IEnumerable<TrackModel> GetTracks() {
            List<TrackModel> trackModels = new List<TrackModel>();
            IEnumerable<TrackView> tracks = _controller.GetTrackViews();
            foreach (TrackView trackView in tracks) {
                TrackModel model = GetTrack(trackView.TrackKey);
                trackModels.Add(model);
            }

            return trackModels;
        }

        private AddressModel GetAddressModel(int addressKey) {
            Address address = _controller.GetAddress(addressKey);
            AddressModel addressModel = new AddressModel() {
                AddressKey = address.AddressKey,
                City = address.City,
                Latitude = address.Latitude == null ? 0 : (decimal) address.Latitude,
                Longitude = address.Longitude == null ? 0 : (decimal) address.Longitude,
                State = address.State,
                StreetAddress1 = address.StreetAddress1,
                StreetAddress2 = address.StreetAddress2,
                ZipCode = address.ZipCode
            };
            return addressModel;
        }

        public void CreateRace(RaceModel raceModel) {
            _controller.CreateRace(raceModel.RaceName, raceModel.TrackKey, raceModel.RaceDate);
        }

        public void UpdateRace(RaceModel raceModel) {
            _controller.UpdateRace(raceModel.RaceKey, raceModel.RaceName, raceModel.TrackKey, raceModel.RaceDate);
        }

        public RaceModel GetRace(int raceKey) {
            RaceModel model = null;
            Race race = _controller.GetRace(raceKey);
            if (race != null) {
                model = new RaceModel() {
                    RaceKey = race.RaceKey,
                    RaceName = race.RaceName,
                    RaceDate = race.RaceDate,
                    TrackKey = race.TrackKey,
                    TrackName = race.Track.TrackName
                };

            }
            return model;
          
        }

        public RaceModel GetRace(DateTime raceDate) {
            RaceModel model = null;
            Race race = _controller.GetRace(raceDate);
            if (race != null) {
                model = new RaceModel() {
                    RaceKey = race.RaceKey,
                    RaceName = race.RaceName,
                    RaceDate = race.RaceDate,
                    TrackKey = race.TrackKey,
                    TrackName = race.Track.TrackName
                };

            }
            return model;
        }

        public IEnumerable<RaceModel> GetYearsRaces(short year) {
            List<RaceModel> models = new List<RaceModel>();
            IEnumerable<Race> races = _controller.GetYearsRaces(year);
            foreach (Race race in races) {
                RaceModel model = new RaceModel() {
                    RaceKey = race.RaceKey,
                    RaceName = race.RaceName,
                    RaceDate = race.RaceDate, 
                    TrackKey = race.TrackKey,
                    TrackName = race.Track.TrackName
                };
                models.Add(model);
            }
            
            return models;
        }

        public IEnumerable<RaceModel> GetRacesWithResults(short year) {
            List<RaceModel> models = new List<RaceModel>();
            IEnumerable<Race> races = _controller.GetYearsRacesThatHaveResults(year);
            foreach (Race race in races) {
                Address addr = _controller.GetAddress(race.Track.AddressKey);
                RaceModel model = new RaceModel() {
                    RaceKey = race.RaceKey,
                    RaceName = race.RaceName,
                    RaceDate = race.RaceDate,
                    TrackKey = race.TrackKey,
                    TrackName = race.Track.TrackName,
                    City = (addr == null ? "" : addr.City)
                };
                models.Add(model);
            }

            return models;
        }

        public IEnumerable<RaceModel> GetRacesWithoutResults(short year) {
            List<RaceModel> models = new List<RaceModel>();
            IEnumerable<Race> races = _controller.GetYearsRacesWithoutResults(year);
            foreach (Race race in races) {
                Address addr = _controller.GetAddress(race.Track.AddressKey);
                RaceModel model = new RaceModel() {
                    RaceKey = race.RaceKey,
                    RaceName = race.RaceName,
                    RaceDate = race.RaceDate,
                    TrackKey = race.TrackKey,
                    TrackName = race.Track.TrackName,
                    City = (addr == null ? "" : addr.City)
                };
                models.Add(model);
            }

            return models;
        }

        public IEnumerable<RaceModel> GetTrackRaces(int trackKey) {
            List<RaceModel> models = new List<RaceModel>();
            IEnumerable<Race> races = _controller.GetTrackRaces(trackKey);
            foreach (Race race in races) {
                RaceModel model = new RaceModel() {
                    RaceKey = race.RaceKey,
                    RaceName = race.RaceName,
                    RaceDate = race.RaceDate
                };
                models.Add(model);
            }

            return models;
        }

        public RaceClassResultEntryModel GetRaceResultEntryModel(int raceKey, int raceClassKey, int raceResultKey, short year = 0) {
            short currentYear = 0;
            RaceClassMemberService raceClassMemberService = new RaceClassMemberService();
            if (year < 1) {
                currentYear = raceClassMemberService.GetCurrentEnrollmentYear();
            }
            RaceClassResultEntryModel entryModel = new RaceClassResultEntryModel();
            RaceModel raceModel = GetRace(raceKey);
            RaceClassModel raceClass = GetRaceClassModel(raceClassKey);
            entryModel.Race = raceModel;
            entryModel.RaceClass = raceClass;
            IEnumerable<RaceClassMemberModel> memberViews = raceClassMemberService.GetRaceClassMemberViews(raceClassKey, currentYear);
            foreach (RaceClassMemberModel member in memberViews) {
                RaceResult raceResult = _controller.GetMemberRaceResult(member.RaceClassMemberKey, raceKey);
                MemberRaceResultEntryModel memberRaceResultEntry =  new MemberRaceResultEntryModel();
                    memberRaceResultEntry.LastName = member.LastName;
                    memberRaceResultEntry.FirstName = member.FirstName;
                    memberRaceResultEntry.RaceClassMemberKey = member.RaceClassMemberKey;
                    memberRaceResultEntry.RaceKey = raceKey;
                    memberRaceResultEntry.RaceClassKey = raceClassKey;
                    if (raceResult != null) {
                        memberRaceResultEntry.MotoOnePosition = raceResult.MotoOnePosition;
                        memberRaceResultEntry.MotoTwoPosition = raceResult.MotoTwoPosition;
                        memberRaceResultEntry.Overall = raceResult.Overall;
                        memberRaceResultEntry.StatusMotoOne = raceResult.StatusMotoOne;
                        memberRaceResultEntry.StatusMotoTwo = raceResult.StatusMotoTwo;
                        // update result key matches allow entry
                        if (raceResultKey == raceResult.RaceResultKey) {
                            memberRaceResultEntry.EntrySuccess = false;
                        } else {
                            memberRaceResultEntry.EntrySuccess = true;
                        }
                        memberRaceResultEntry.RaceResultKey = raceResult.RaceResultKey;
                        
                    } else {
                        memberRaceResultEntry.EntrySuccess = false;
                    }
                    entryModel.MemberResultEntries.Add(memberRaceResultEntry);
            }

            return entryModel;
        }      
       



        public bool CreateMemberRaceResult(MemberRaceResultEntryModel raceResultModel) {
            return _controller.CreateRaceResult(raceResultModel.RaceClassMemberKey, raceResultModel.RaceKey, 
                raceResultModel.MotoOnePosition, raceResultModel.StatusMotoOne, raceResultModel.MotoTwoPosition, 
                raceResultModel.StatusMotoTwo, raceResultModel.Overall, raceResultModel.RidingNumber, 
                raceResultModel.Brand, raceResultModel.Note);
        }

        public void UpdateMemberRaceResultDetails(int raceResultKey, string ridingNumber, string brand, string note = "") {
            _controller.UpdateRaceResultDetails(raceResultKey, ridingNumber, brand, note);
        }

        public void UpdateMemberRaceResultPosition(int raceResultKey, short motoOnePosition, string motoOneStatus, short motoTwoPosition, string motoTwoStatus, short overall) {
            _controller.UpdateRaceResultPosition(raceResultKey, motoOnePosition, motoOneStatus, motoTwoPosition, motoTwoStatus, overall);
        }

        //public IEnumerable<RaceResultModel> GetMemberRaceResults(int memberKey, short year) {
        //    IEnumerable<IGrouping<int, RaceClassMemberRaceResult>> groupedRaceResults = _controller.GetMemberRaceResults(memberKey, year);
        //    return GetGroupedRaceResultModels(groupedRaceResults);
        //}

        

        //public RaceResultModel GetClassRaceResult(int raceKey, int raceClassKey) {
            
        //    RaceResultModel model = new RaceResultModel();
        //    model.RaceClassResults = new List<RaceClassResultModel>();
        //    RaceClassResultModel raceClassResultModel = new RaceClassResultModel();
        //    raceClassResultModel.RaceClass = GetRaceClassModel(raceClassKey);
        //    model.Race = GetRace(raceKey);
        //    model.Track = GetTrack(model.Race.TrackKey);
           
        //    IEnumerable<RaceClassMemberRaceResult> raceResults = _controller.GetClassRaceResult(raceKey, raceClassKey);
        //    foreach (RaceClassMemberRaceResult r in raceResults) {
        //        short age = _classController.GetRaceMemberAge(r.MemberKey);
        //        MemberRaceResultModel memberRaceResultModel = new MemberRaceResultModel() {
        //            Age = age,
        //            LicenseNumber = r.LicenseNumber,
        //            Brand = r.Brand,
        //            City = r.City,
        //            State = r.State,
        //            FirstName = r.FirstName,
        //            MiddleName = r.MiddleName,
        //            LastName = r.LastName,
        //            MotoOnePosition = r.MotoOnePosition,
        //            MotoTwoPosition = r.MotoTwoPosition,
        //            Overall = r.Overall,
        //            Points = r.Points,
        //            RidingNumber = r.RidingNumber,
        //            RaceKey = r.RaceKey,
        //            RaceClassMemberKey = r.RaceClassMemberKey,
        //            RaceResultKey = r.RaceResultKey,
        //            Note = r.Note
        //        };
        //        raceClassResultModel.MemberResults.Add(memberRaceResultModel);
        //    }

        //    model.RaceClassResults.Add(raceClassResultModel);
        //    return model;

        //}

        //public IEnumerable<RaceResultModel> GetClassRaceResults(int raceClassKey, short year) {
        //    IEnumerable<IGrouping<int, MemberRaceResultsView>> groupedRaceResults = _controller.GetClassRaceResults(raceClassKey, year);
        //    return GetGroupedRaceResultModels(groupedRaceResults);

        //}

        //public IEnumerable<RaceResultModel> GetAllRaceResults(short year) {
        //    IEnumerable<IGrouping<int, MemberRaceResultsView>> groupedRaceResults = _controller.GetAllRaceResultsForYear(year);
        //    return GetGroupedRaceResultModels(groupedRaceResults);
        //}       

        //private IEnumerable<RaceResultModel> GetGroupedRaceResultModels(IEnumerable<IGrouping<int, RaceClassMemberRaceResult>> groupedRaceResults) {
        //    List<RaceResultModel> raceResults = new List<RaceResultModel>();
        //    foreach (IGrouping<int, RaceClassMemberRaceResult> outerGroup in groupedRaceResults) {
        //        IEnumerable<IGrouping<int, RaceClassMemberRaceResult>> innerGroups = outerGroup.GroupBy(og => og.RaceClassKey);
        //        RaceResultModel model = SetRaceResultsModel(innerGroups, outerGroup.Key);
        //        raceResults.Add(model);
        //    }
        //    return raceResults;
        //}

        //private IEnumerable<RaceResultModel> GetGroupedRaceResultModels(IEnumerable<IGrouping<int, MemberRaceResultsView>> groupedRaceResults) {
        //    List<RaceResultModel> raceResults = new List<RaceResultModel>();
        //    foreach (IGrouping<int, MemberRaceResultsView> outerGroup in groupedRaceResults) {
        //        IEnumerable<IGrouping<int, MemberRaceResultsView>> innerGroups = outerGroup.GroupBy(og => og.RaceClassKey);
        //        RaceResultModel model = SetMemberRaceResultsModel(innerGroups, outerGroup.Key);
        //        raceResults.Add(model);
        //    }
        //    return raceResults;
        //}


        //private RaceResultModel SetRaceResultsModel(IEnumerable<IGrouping<int, RaceClassMemberRaceResult>> groupedRaceResults, int raceKey) {
            
        //    RaceResultModel model = new RaceResultModel();
        //    model.RaceClassResults = new List<RaceClassResultModel>();
           
        //    foreach (IGrouping<int, RaceClassMemberRaceResult> group in groupedRaceResults) {

        //        model.Race = GetRace(raceKey);
        //        if (model.Race != null) {
        //            model.Track = GetTrack(model.Race.TrackKey);

        //            foreach (RaceClassMemberRaceResult r in group) {

        //                short age = _classController.GetRaceMemberAge(r.MemberKey);
        //                MemberRaceResultModel memberRaceResultModel = new MemberRaceResultModel() {
        //                    Age = age,
        //                    LicenseNumber = r.LicenseNumber,
        //                    Brand = r.Brand,
        //                    City = r.City,
        //                    State = r.State,
        //                    FirstName = r.FirstName,
        //                    MiddleName = r.MiddleName,
        //                    LastName = r.LastName,
        //                    MotoOnePosition = r.MotoOnePosition,
        //                    MotoTwoPosition = r.MotoTwoPosition,
        //                    Overall = r.Overall,
        //                    Points = r.Points,
        //                    RidingNumber = r.RidingNumber,
        //                    RaceKey = r.RaceKey,
        //                    RaceClassMemberKey = r.RaceClassMemberKey,
        //                    RaceResultKey = r.RaceResultKey,
        //                    Note = r.Note
        //                };
        //                if (RaceClassResultModels.ContainsKey(r.RaceClassKey)) {
        //                    RaceClassResultModels[r.RaceClassKey].MemberResults.Add(memberRaceResultModel);
        //                }


        //            }
        //            foreach (RaceClassResultModel raceClassResultModel in RaceClassResultModels.Values) {
        //                if (raceClassResultModel.MemberResults.Count > 0) {
        //                    model.RaceClassResults.Add(raceClassResultModel);
        //                }
        //            }
        //        }
        //        RaceClassResultModels = null;
        //    }
        //    return model;
        //}

        //private RaceResultModel SetMemberRaceResultsModel(IEnumerable<IGrouping<int, MemberRaceResultsView>> groupedRaceResults, int raceKey) {
            
        //    RaceResultModel model = new RaceResultModel();
        //    model.RaceClassResults = new List<RaceClassResultModel>();

        //    foreach (IGrouping<int, MemberRaceResultsView> group in groupedRaceResults) {

        //        model.Race = GetRace(raceKey);
        //        if (model.Race != null) {
        //            model.Track = GetTrack(model.Race.TrackKey);

        //            foreach (MemberRaceResultsView r in group) {

        //                short age = _classController.GetRaceMemberAge(r.MemberKey);
        //                MemberRaceResultModel memberRaceResultModel = new MemberRaceResultModel() {
        //                    Age = age,
        //                    Brand = r.Brand,
        //                    FirstName = r.FirstName,
        //                    LastName = r.LastName,
        //                    MotoOnePosition = r.MotoOnePosition,
        //                    MotoTwoPosition = r.MotoTwoPosition,
        //                    Overall = r.Overall,
        //                    Points = r.Points,
        //                    RidingNumber = r.RidingNumber,
        //                    RaceKey = r.RaceKey,
        //                    RaceClassMemberKey = r.RaceClassMemberKey,
        //                    RaceResultKey = r.RaceResultKey,
        //                    Note = r.Note,
        //                    PointsTotal = (r.PointsTotal == null ? (short)0 : (short) r.PointsTotal),
        //                    PointsDropped = (r.PointsDropped == null ? (short)0 : (short)r.PointsDropped)

                            
        //                };
        //                if (RaceClassResultModels.ContainsKey(r.RaceClassKey)) {
        //                    RaceClassResultModels[r.RaceClassKey].MemberResults.Add(memberRaceResultModel);
        //                }


        //            }
        //            foreach (RaceClassResultModel raceClassResultModel in RaceClassResultModels.Values) {
        //                if (raceClassResultModel.MemberResults.Count > 0) {
        //                    model.RaceClassResults.Add(raceClassResultModel);
        //                }
        //            }
        //        }
        //        RaceClassResultModels = null;
        //    }
        //    return model;
        //}


        //private Dictionary<int, RaceClassResultModel> raceClassResultModels;
        //private Dictionary<int, RaceClassResultModel> RaceClassResultModels {
        //    get {
        //        if (raceClassResultModels == null) {
        //            raceClassResultModels = new Dictionary<int, RaceClassResultModel>();
        //            IEnumerable<RaceClassModel> raceClasses = GetAllRaceClassModels();
        //            foreach (RaceClassModel model in raceClasses) {
        //                RaceClassResultModel rcrm = new RaceClassResultModel();
        //                rcrm.RaceClass = model;
        //                raceClassResultModels.Add(model.RaceClassKey, rcrm);
        //            }

        //        }

        //        return raceClassResultModels;
        //    }
        //    set {
        //        raceClassResultModels = value;
        //    }

        //}      


        private static RaceClassModel GetRaceClassModel(int raceClassKey) {
            RaceClassMemberService raceClassMemberService = new RaceClassMemberService();           
            return raceClassMemberService.GetRaceClass(raceClassKey);

        }

        private static IEnumerable<RaceClassModel> GetAllRaceClassModels() {
            RaceClassMemberService raceClassMemberService = new RaceClassMemberService();
            RaceClassModels rcm = raceClassMemberService.GetAllRaceClasses();
            if (rcm != null) {
                return rcm.Models;
            } else {
                return null;
            }
        }



        public bool ProcessRaceResultFile(string serverFilePath, int raceKey, string worksheetName, int year = 0) {
            RaceResultFileProcessor processor = new RaceResultFileProcessor();
            if (year == 0) {
                DateTime now = DateTime.Now;
                year = now.Year;
            }
            
            processor.ProcessRaceResultFile(serverFilePath, raceKey, worksheetName, year);

            return true;
        }


        public IEnumerable<RaceClassDisplayModel> GetRaceClassesForResults(int raceKey) {
            IEnumerable<RaceClass> raceClasses = _controller.GetRaceClassesForRace(raceKey);
            List<RaceClassDisplayModel> raceClassModels = new List<RaceClassDisplayModel>();
            foreach (RaceClass rc in raceClasses) {
                RaceClassDisplayModel model = new RaceClassDisplayModel() {
                    RaceKey = raceKey,
                    ClassName = rc.ClassName,
                    Enabled = rc.Enabled,
                    Note = rc.Note,
                    RaceClassKey = rc.RaceClassKey
                };
                raceClassModels.Add(model);

            }

            return raceClassModels;
        }

        public IEnumerable<RaceClassMemberResultModel> GetRaceResultsForRaceClass(int raceKey, int raceClassKey) {
            IEnumerable<RaceClassMemberResultView> resultsViews = _controller.GetRaceClassResultsForRaceClass(raceKey, raceClassKey);
            List<RaceClassMemberResultModel> models = new List<RaceClassMemberResultModel>();
            foreach (RaceClassMemberResultView rcmr in resultsViews) {
                short age = _classController.GetRaceMemberAge((int)rcmr.MemberKey);
                RaceClassMemberResultModel model = new RaceClassMemberResultModel() {
                    Age = age,
                    Brand = rcmr.Brand,
                    FirstName = rcmr.FirstName,
                    LastName = rcmr.LastName,
                    MemberKey = (int)rcmr.MemberKey,
                    MotoOnePosition =  SetMotoResult(rcmr.MotoOnePosition, rcmr.StatusMotoOne),
                    MotoTwoPosition = SetMotoResult(rcmr.MotoTwoPosition, rcmr.StatusMotoTwo),
                    Note = rcmr.Note,
                    Overall = rcmr.Overall,
                    Points = rcmr.Points,
                    PointsDropped = (short) (rcmr.PointsDropped == null ? 0 : rcmr.PointsDropped),
                    PointsTotal = (short) (rcmr.PointsTotal == null ? 0 : rcmr.PointsTotal),
                    RaceKey = rcmr.RaceKey,
                    RaceClassMemberKey = rcmr.RaceClassMemberKey,
                    RaceResultKey = rcmr.RaceResultKey,
                    RidingNumber = rcmr.RidingNumber,
                    StatusMotoOne = rcmr.StatusMotoOne,
                    StatusMotoTwo = rcmr.StatusMotoTwo                   

                };

                models.Add(model);

            }
            return models;
        }

        private string SetMotoResult(short position, string status) {
            if (status.Equals("Finished", StringComparison.InvariantCultureIgnoreCase)) {
                return position.ToString();

            } else {
                return status;
            }

        }

        public void SetPointsDropForEnrollmentYear() {
            _classController.SetPointsDropForEnrollmentYear();
            _controller.DropPoints();           
            
        }
    }
}
