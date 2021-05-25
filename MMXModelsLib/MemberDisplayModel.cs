using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MMXModelsLib {
    public class MemberDisplayModel : BaseModel {
        
        public int MemberKey { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Middle Name")]
        public string MiddleName { get; set; }
                
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Suffix")]
        public string Suffix { get; set; }

        [DisplayName("Classes")]
        public string Classes { get; set; }   

        

    }
}
