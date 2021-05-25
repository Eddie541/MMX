using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MMXModelsLib {
    public class RaceYearResultModel {

        [DisplayName("Year")]
        public short Year { get; set; }


        public List<RaceModel> RaceModels { get; set; }
    }
}
