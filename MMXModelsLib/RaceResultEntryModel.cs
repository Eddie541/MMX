using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMXModelsLib {
    public class RaceResultEntryModel : BaseModel {
        public RaceModel Race { get; set; }
        public IEnumerable<RaceClassModel> Classes { get; set; }
    }
}
