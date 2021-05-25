using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MMXModelsLib;

namespace MMXServiceInterfaceLib {
    public interface IRaceService {
        int CreateTrack(TrackModel trackModel);
        void UpdateTrack(TrackModel trackModel);
        void CloseTrack(int trackKey);
        TrackModel GetTrack(int trackKey);
        IEnumerable<TrackModel> GetTracks();

        void CreateRace(RaceModel raceModel);
        void UpdateRace(RaceModel raceModel);
        RaceModel GetRace(int raceKey);
        RaceModel GetRace(DateTime raceDate);
        IEnumerable<RaceModel> GetYearsRaces(short year);
        IEnumerable<RaceModel> GetTrackRaces(int trackKey);
        RaceClassResultEntryModel GetRaceResultEntryModel(int raceKey, int raceClassKey, int resultKey, short year = 0);
        bool CreateMemberRaceResult(MemberRaceResultEntryModel raceResultModel);
        void UpdateMemberRaceResultDetails(int raceResultKey, string ridingNumber, string brand, string note = "");
        void UpdateMemberRaceResultPosition(int raceResultKey, short motoOnePosition, string motoOneStatus, short motoTwoPosition, string motoTwoStatus, short overall);
        //IEnumerable<RaceResultModel> GetMemberRaceResults(int memberKey, short year);
        //RaceResultModel GetClassRaceResult(int raceKey, int raceClassKey);
        //IEnumerable<RaceResultModel> GetClassRaceResults(int raceClassKey, short year);
        //IEnumerable<RaceResultModel> GetAllRaceResults(short year);
        bool ProcessRaceResultFile(string serverFilePath, int raceKey, string worksheetName, int year);
        IEnumerable<RaceModel> GetRacesWithResults(short year);
        IEnumerable<RaceClassDisplayModel> GetRaceClassesForResults(int raceKey);
        IEnumerable<RaceClassMemberResultModel> GetRaceResultsForRaceClass(int raceKey, int raceClassKey);
        void SetPointsDropForEnrollmentYear();
        IEnumerable<RaceModel> GetRacesWithoutResults(short year);
    }

    public enum StartFinishStatus {
        [Description("DNF")] 
        DidNotFinish = 100,
        [Description("DNS")] 
        DidNotStart = 200,
        [Description("DSQ")] 
        Disqualified = 300
    }
}
