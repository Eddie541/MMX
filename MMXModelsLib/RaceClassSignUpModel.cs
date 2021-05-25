using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMXModelsLib {
    public class RaceClassSignUpModel : BaseModel {
        public int RaceClassKey { get; set; }
        public int MotorcycleKey { get; set; }
        public RaceClassModel SelectedRaceClassModel { get; set; }
        public List<MotorcycleModel> Motorcycles { get; set; }
        public List<RaceClassModel> Classes { get; set; }

    }
}
