using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMXModelsLib {
    public class RaceClassResultEntryModel : BaseModel {

        public RaceModel Race { get; set; }

        public RaceClassModel RaceClass { get; set; }

        public List<MemberRaceResultEntryModel> MemberResultEntries { get; set; }

        public RaceClassResultEntryModel() {
            MemberResultEntries = new List<MemberRaceResultEntryModel>();
        }

        public void AddModelEntryErrors(int raceClassMemberKey, List<ErrorMessageModel> errors) {
            if (MemberResultEntries.Any(m => m.RaceClassMemberKey == raceClassMemberKey)) {
                (MemberResultEntries.Where(m => m.RaceClassMemberKey == raceClassMemberKey).Single()).Errors = errors;
                (MemberResultEntries.Where(m => m.RaceClassMemberKey == raceClassMemberKey).Single()).EntrySuccess = false;
            }

        }

        //public List<FinishSelection> FinishSelections {
        //    get {
        //        List<FinishSelection> fModel = new List<FinishSelection>();
        //        fModel.Add(new FinishSelection("Finished"));
        //        fModel.Add(new FinishSelection("DNF"));
        //        fModel.Add(new FinishSelection("DNS"));
        //        return fModel;
        //    }
        //}

    }
}
