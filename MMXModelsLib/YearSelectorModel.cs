using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMXModelsLib {
    public class YearSelectorModel : BaseModel {

        public short CurrentYear { get; set; }
        public List<SelectorKey> Years { get; set; }        

    }

    public class SelectorKey {
        public short CurrentYear { get; set; }

    }
}
