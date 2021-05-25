using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMXModelsLib {
    public class RaceSelectorModel : BaseModel {

        public int RaceKey { get; set; }

        public short Year { get; set; }

        public IEnumerable<RaceModel> Races { get; set; }


    }
}
