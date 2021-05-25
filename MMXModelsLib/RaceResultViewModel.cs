using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMXModelsLib {
    public class RaceResultViewModel : BaseModel {
        public short Year { get; set; }
        public List<RaceModel> CompletedRaces { get; set; }
        public YearSelectorModel YearSelector { get; set; }
    }

    public class UploadRaceResultsViewModel : BaseModel {
        public SelectedRaceUploadModel SelectedRaceModel { get; set; }

        public UploadRaceResultsViewModel() {
            SelectedRaceModel = new SelectedRaceUploadModel();
        }
    }
}
