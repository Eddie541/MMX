using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MMXModelsLib {
    public class RaceClassModel : BaseModel {

        public int RaceClassKey { get; set; }              

        [Required]
        [DisplayName("Class Name")]
        public string ClassName { get; set; }

        [DisplayName("Minimum Age")]
        public short MinimumAge { get; set; }

        [DisplayName("Note")]
        public string Note { get; set; }

        public bool Enabled { get; set; }

        public bool ShowingAll { get; set; }

        public List<RaceClassMemberModel> Members { get; set; }


    }

    public class RaceClassModels : BaseModel {

        public RaceClassModels() {
            Models = new List<RaceClassModel>();           
        }
        public bool ShowAll { get; set; }
        
        public List<RaceClassModel> Models {
            get;
            set;
        }

        
           
    }
}
