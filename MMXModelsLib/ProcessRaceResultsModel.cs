﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMXModelsLib {
    public class ProcessRaceResultsModel {

        public IEnumerable<RaceModel> Races { get; set; }

        public string FileName { get; set; }

        public string WorkSheetName { get; set; }


    }
}
