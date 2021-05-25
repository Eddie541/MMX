using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMXModelsLib {
    public class SelectRaceResultUploadModel : BaseModel {

        public SelectedRaceUploadModel SelectedRaceModel { get; set; }

        public IEnumerable<RaceModel> Races { get; set; }
        public List<SelectorKey> Years { get; set; }

        public SelectRaceResultUploadModel() {
            SelectedRaceModel = new SelectedRaceUploadModel();
        }
        
    }
}
