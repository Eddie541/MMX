using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMXModelsLib {
    public class RaceClassDisplayListModel : BaseModel {

        private int raceKey = 0;
        public int RaceKey { get { return raceKey; } set { raceKey = value; } }
        public short Year { get; set; }

        public List<RaceClassDisplayModel> RaceClassDisplayModels { get; set; }

    }
}
