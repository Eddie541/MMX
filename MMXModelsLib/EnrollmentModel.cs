using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MMXModelsLib {
    public class EnrollmentModel : BaseModel {

        [DisplayName("Enrollment Year")]
        public short EnrollmentYear { get; set; }

        [DisplayName("Open")]       
        public bool EnrollmentOpen { get; set; }

        [DisplayName("Drop Points")]
        public bool DropPoints { get; set; }

    }
}
