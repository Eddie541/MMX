using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace MMXDataLib {
    public class RaceDataController : BaseDataController {


        public RaceDataController(string testConn = "") :
            base(testConn) {
        }

        public int CreateTrack(string name, string contactName, string phone, string emailAddress, string webAddress, int addressKey) {
            Track track = new Track() {
                TrackName = name,
                ContactName = contactName,
                Phone = phone,
                EmailAddress = emailAddress,
                WebAddress = webAddress,
                AddressKey = addressKey,
                IsOpen = true

            };

            context.Tracks.AddObject(track);
            context.SaveChanges();

            return track.TrackKey;
        }

        public void UpdateTrack(int trackKey, string name, string contactName, string phone, string emailAddress, string webAddress, bool isOpen = true) {
            Track track = context.Tracks.Where(t => t.TrackKey == trackKey).Single();
            track.TrackName = name;
            track.ContactName = contactName;
            track.Phone = phone;
            track.EmailAddress = emailAddress;
            track.WebAddress = webAddress;
            track.IsOpen = isOpen;
            context.SaveChanges();
        }

        public void CloseTrack(int trackKey) {
            Track track = context.Tracks.Where(t => t.TrackKey == trackKey).Single();
            track.IsOpen = false;
            context.SaveChanges();
        }

        public void CreateRace(string raceName, int trackKey, DateTime raceDate) {
            Race race = new Race() {
                RaceName = raceName,
                TrackKey = trackKey,
                RaceDate = raceDate
            };

            context.Races.AddObject(race);
            context.SaveChanges();
        }

        public void UpdateRace(int raceKey, string raceName, int trackKey, DateTime raceDate) {
            Race race = context.Races.Where(r => r.RaceKey == raceKey).Single();
            race.TrackKey = trackKey;
            race.RaceDate = raceDate;
            race.RaceName = raceName;

            context.SaveChanges();
        }

        // todo stored proc to check and safely remove an unused race
        //public void DeleteRace(int raceKey) {
        //    Race race = context.Races.Where(r => r.RaceKey == raceKey).Single();
        //    context.DeleteObject(race);
        //    context.SaveChanges();
        //}

        public Race GetRace(int raceKey) {
            Race race = null;
            try {
                race = context.Races.Where(r => r.RaceKey == raceKey).Single();
            } catch {

            }
            return race;
        }

        public Race GetRace(DateTime raceDate) {
            Race race = null;
            try {
                IQueryable<Race> races = from r in context.Races
                                         where EntityFunctions.TruncateTime(raceDate) == r.RaceDate
                                         select r;

                race = races.FirstOrDefault();
            } catch { }

            return race;
        }

        public IEnumerable<Race> GetYearsRaces(int year) {
            IEnumerable<Race> races = context.Races.Where(r => r.RaceDate.Year == year);
            
            return races;
        }

        public IEnumerable<Race> GetYearsRacesThatHaveResults(int year) {
            IEnumerable<Race> races = context.GetRacesWithResultsForYear((short)year);
            return races;
        }

        public IEnumerable<Race> GetYearsRacesWithoutResults(int year) {
            IEnumerable<Race> races = context.GetRacesWithoutResultsForYear((short)year);
            return races;
        }

        public IEnumerable<RaceClass> GetRaceClassesForRace(int raceKey) {
            IEnumerable<RaceClass> rcs = context.GetRaceClassesForCompletedRace((int?)raceKey);
            return rcs;
        }

        public IEnumerable<RaceClassMemberResultView> GetRaceClassResultsForRaceClass(int raceKey, int raceClassKey) {
            IEnumerable<RaceClassMemberResultView> rcmr = context.RaceClassMemberResultViews.Where(cr => cr.RaceKey == raceKey && cr.RaceClassKey == raceClassKey).OrderByDescending(rr => rr.Points).ThenBy(rr => rr.MotoTwoPosition);
            return rcmr;
        }

        public IEnumerable<Race> GetTrackRaces(int trackKey) {
            IEnumerable<Race> races = context.Races.Where(r => r.TrackKey == trackKey);
            return races;
        }

        public bool AreResultsDuplicated(int raceKey, int raceClassKey, short motoOnePosition, short motoTwoPosition, short overall) {
            bool duplicateResults = false;            
            ErrorCollection.Clear();
            try {
                IEnumerable<RaceClassMemberRaceResult> raceResults = context.RaceClassMemberRaceResults.Where(rr => rr.RaceKey == raceKey && rr.RaceClassKey == raceClassKey);
                foreach (RaceClassMemberRaceResult raceResult in raceResults) {
                    if (raceResult != null) {
                        if (raceResult.MotoOnePosition > 0 && raceResult.MotoOnePosition == motoOnePosition) {
                            ErrorCollection.Add(string.Format("Moto One Position {0} has already been entered", motoOnePosition));
                            duplicateResults = true;
                        }
                        if (raceResult.MotoTwoPosition > 0 && raceResult.MotoTwoPosition == motoTwoPosition) {
                            ErrorCollection.Add(string.Format("Moto Two Position {0} has already been entered", motoTwoPosition));
                            duplicateResults = true;
                        }
                        if (raceResult.Overall > 0 && raceResult.Overall == overall) {
                            ErrorCollection.Add(string.Format("Overall position {0} has already been entered", overall));
                            duplicateResults = true;
                        }
                    }
                }
            } catch {
                duplicateResults = false;
            }

            return duplicateResults;
        }

        public bool CreateRaceResult(int raceClassMemberKey, int raceKey, short motoOnePosition, string motoOneStatus,
            short motoTwoPosition, string motoTwoStatus, short overall, string ridingNumber, string brand, string note = "") {
            ErrorCollection.Clear();
            bool success = false;
            if ((motoOnePosition == 0 && (motoOneStatus.Equals("Finished") || string.IsNullOrEmpty(motoOneStatus)))
                || (motoTwoPosition == 0 && (motoTwoStatus.Equals("Finished") || string.IsNullOrEmpty(motoTwoStatus)))) {
                return success;
            }
            
            Race race = GetRace(raceKey);
            if (race != null) {
                // todo re add this validation
                //if (DateTime.Now < race.RaceDate) {
                // ErrorCollection.Add("Results cannot be entered for future race dates");
                //    return sucess;
                //}
                short points = GetPositionPoints(motoOnePosition);
                points += GetPositionPoints(motoTwoPosition);
                RaceResult result = new RaceResult() {
                    RaceKey = raceKey,
                    RaceClassMemberKey = raceClassMemberKey,
                    MotoOnePosition = motoOnePosition,
                    MotoTwoPosition = motoTwoPosition,
                    Points = points,
                    Overall = overall,
                    RidingNumber = ridingNumber,
                    Brand = brand,
                    StatusMotoOne = motoOneStatus,
                    StatusMotoTwo = motoTwoStatus,
                    Note = note

                };

                context.RaceResults.AddObject(result);
                context.SaveChanges();
                success = true;                
                context.SetPointTotals(raceClassMemberKey);
                SetDroppedPoints(raceClassMemberKey);
             
            } else {
                ErrorCollection.Add("Invalid Race");
                success = false;                     
            }
            return success;
        }

        public void UpdateRaceResultDetails(int raceResultKey, string ridingNumber, string brand, string note = "") {
            RaceResult result = context.RaceResults.Where(r => r.RaceResultKey == raceResultKey).Single();
            result.RidingNumber = ridingNumber;
            result.Brand = brand;
            result.Note = note;
            context.SaveChanges();
        }

        public void UpdateRaceResultPosition(int raceResultKey, short motoOnePosition, string motoOneStatus,
            short motoTwoPosition, string motoTwoStatus, short overall) {
            RaceResult result = context.RaceResults.Where(r => r.RaceResultKey == raceResultKey).Single();
            short points = GetPositionPoints(motoOnePosition);
            points += GetPositionPoints(motoTwoPosition);
            result.Points = points;
            result.MotoOnePosition = motoOnePosition;
            result.MotoTwoPosition = motoTwoPosition;
            result.StatusMotoOne = motoOneStatus;
            result.StatusMotoTwo = motoTwoStatus;
            result.Overall = overall;
            context.SaveChanges();
            int raceClassMemberKey = result.RaceClassMemberKey;
            context.SetPointTotals(raceClassMemberKey);
            // todo conditionally apply drops
            SetDroppedPoints(raceClassMemberKey);
        }

        private static void SetDroppedPoints(int raceClassMemberKey) {
            Enrollment enrollment = context.Enrollments.Where(en => en.EnrollmentKey == 1).Single();
            if (enrollment.DropPoints) {
                context.SetPointsDropped(raceClassMemberKey);
            }
        }


        public void DropPoints() {
            context.SetAllRaceClassMemberDroppedPoints();
        }

        public RaceResult GetMemberRaceResult(int raceClassMemberKey, int raceKey) {
            RaceResult raceResult = null;
            try {
                raceResult = context.RaceResults.Where(r => r.RaceClassMemberKey == raceClassMemberKey && r.RaceKey == raceKey).Single();

            } catch {

            }
            return raceResult;
        }

        public TrackView GetTrackView(int trackKey) {
            TrackView trackView = context.TrackViews.Where(t => t.TrackKey == trackKey).Single();
            return trackView;
        }

        public IEnumerable<TrackView> GetTrackViews() {
            IEnumerable<TrackView> trackViews = context.TrackViews.Select(t => t);
            return trackViews;
        }

        public Track GetTrack(int trackKey) {
            Track track = context.Tracks.Where(t => t.TrackKey == trackKey).Single();
            return track;
        }

        public IEnumerable<RaceClassMemberRaceResult> GetClassRaceResult(int raceKey, int raceClassKey) {
            IEnumerable<RaceClassMemberRaceResult> raceClassResults = context.RaceClassMemberRaceResults.Where(r => r.RaceKey == raceKey && r.RaceClassKey == raceClassKey).OrderBy(r => r.Overall);
            return raceClassResults;

        }

        public IEnumerable<IGrouping<int, MemberRaceResultsView>> GetClassRaceResults(int raceClassKey, short year = 0) {
            if (year == 0) {
                Enrollment enrollment = context.Enrollments.Where(en => en.EnrollmentOpen).First();
                year = enrollment.Year;
            }
            IEnumerable<IGrouping<int, MemberRaceResultsView>> raceClassResults = context.MemberRaceResultsViews.Where(r => r.RaceClassKey == raceClassKey && r.Year == year).OrderBy(r => r.Overall).GroupBy(r => r.RaceKey);
            return raceClassResults;

        }

        public IEnumerable<IGrouping<int, RaceClassMemberRaceResult>> GetMemberRaceResults(int memberKey, short year = 0) {
            if (year == 0) {
                Enrollment enrollment = context.Enrollments.Where(en => en.EnrollmentOpen).First();
                year = enrollment.Year;
            }
            IEnumerable<IGrouping<int, RaceClassMemberRaceResult>> raceClassResults = context.RaceClassMemberRaceResults.Where(r => r.MemberKey == memberKey && r.Year == year).GroupBy(r => r.RaceKey);
            return raceClassResults;

        }

        public IEnumerable<IGrouping<int, MemberRaceResultsView>> GetAllRaceResultsForYear(short year = 0) {
            if (year == 0) {
                Enrollment enrollment = context.Enrollments.Where(en => en.EnrollmentOpen).First();
                year = enrollment.Year;
            }

            IEnumerable<IGrouping<int, MemberRaceResultsView>> raceClassResults = context.MemberRaceResultsViews.Where(r => r.Year == year).OrderBy(r => r.Overall).GroupBy(r => r.RaceKey);
            return raceClassResults;

        }

        public Address GetAddress(int addressKey) {
            return context.Addresses.Where(a => a.AddressKey == addressKey).FirstOrDefault();
        }

        private static short GetPositionPoints(short position) {
            short points = 0;
            switch (position) {
                case 1:
                    points = 25;
                    break;
                case 2:
                    points = 22;
                    break;
                case 3:
                    points = 20;
                    break;
                case 4:
                    points = 18;
                    break;
                case 5:
                    points = 16;
                    break;
                case 6:
                    points = 15;
                    break;
                case 7:
                    points = 14;
                    break;
                case 8:
                    points = 13;
                    break;
                case 9:
                    points = 12;
                    break;
                case 10:
                    points = 11;
                    break;
                case 11:
                    points = 10;
                    break;
                case 12:
                    points = 9;
                    break;
                case 13:
                    points = 8;
                    break;
                case 14:
                    points = 7;
                    break;
                case 15:
                    points = 6;
                    break;
                case 16:
                    points = 5;
                    break;
                case 17:
                    points = 4;
                    break;
                case 18:
                    points = 3;
                    break;
                case 19:
                    points = 2;
                    break;
                case 20:
                    points = 1;
                    break;
                default:
                    points = 0;
                    break;
            }

            return points;

        }

        private static string GetFinishedStatus(short position) {
            string finishedStatus = "Finished";
            switch (position) {
                case 100:
                    finishedStatus = "DNF";
                    break;
                case 200:
                    finishedStatus = "DNS";
                    break;
                case 300:
                    finishedStatus = "DSQ";
                    break;
            }
            return finishedStatus;
        }


    }
}

