using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MMXModelsLib {
    public class RaceResultModel : BaseModel {

        public TrackModel Track { get; set; }

        public RaceModel Race { get; set; }

        public List<RaceClassResultModel> RaceClassResults { get; set; }
    }
}
