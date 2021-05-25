using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MMXModelsLib {
    public class RaceClassDisplayModel : BaseModel {

        public int RaceClassKey { get; set; }

        public int RaceKey { get; set; }

        [DisplayName("Class Name")]
        public string ClassName { get; set; }

        [DisplayName("Note")]
        public string Note { get; set; }

        public bool Enabled { get; set; }
    }
}
