using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMXModelsLib {
    public class RaceClassResultModel : BaseModel {

        public RaceClassResultModel() {
            MemberResults = new List<MemberRaceResultModel>();
        }

        public RaceClassModel RaceClass { get; set; }

        public List<MemberRaceResultModel> MemberResults { get; set; }
    }
}
