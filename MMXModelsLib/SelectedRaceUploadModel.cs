using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMXModelsLib {
    public class SelectedRaceUploadModel : BaseModel {

        public string WorksheetName { get; set; }
        public short CurrentYear { get; set; }
        public string SelectedRace { get; set; }
        public int RaceKey { get; set; }
        
    }
}
