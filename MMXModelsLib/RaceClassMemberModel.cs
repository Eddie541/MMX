using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MMXModelsLib {
    public class RaceClassMemberModel : BaseModel {

        public int RaceClassKey { get; set; }
        public int RaceClassMemberKey { get; set; }
        public int Year { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Middle")]
        public string MiddleName { get; set; }
                
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Suffix")]
        public string Suffix { get; set; }

        [DisplayName("Age")]
        public short Age { get; set; }

        [DisplayName("City")]
        public string City { get; set; }

        [DisplayName("State")]
        public string State { get; set; }

        [DisplayName("Paid")]
        public bool HasPaid { get; set; }

        [DisplayName("Points")]
        public short PointsTotal { get; set; }

        [DisplayName("Points Dropped")]
        public short PointsDropped { get; set; }

        [DisplayName("Total Points")]
        public short AdjustedPointsTotal { get; set; }


       


    }
}
