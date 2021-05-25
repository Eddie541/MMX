using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMXModelsLib {
    public class RaceScheduleModel : BaseModel {
                
        public short Year { get; set; }
        public YearSelectorModel YearSelector { get; set; }

        public IEnumerable<TrackModel> Tracks { get; set; }

        public IEnumerable<RaceModel> Races { get; set; }

    }
}
