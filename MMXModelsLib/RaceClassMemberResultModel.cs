using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MMXModelsLib {
    public class RaceClassMemberResultModel : BaseModel {

        public int RaceResultKey { get; set; }

        public int RaceKey { get; set; }

        public int RaceClassMemberKey { get; set; }

        public int MemberKey { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        //[DisplayName("Middle")]
        //public string MiddleName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        //[DisplayName("City")]
        //public string City { get; set; }

        //[DisplayName("State")]
        //public string State { get; set; }

        [DisplayName("Age")]
        public short Age { get; set; }

        //[DisplayName("License Number")]
        //public string LicenseNumber { get; set; }

        [DisplayName("Brand")]
        public string Brand { get; set; }

        [DisplayName("Riding Number")]
        public string RidingNumber { get; set; }

        [DisplayName("Moto One")]
        public string MotoOnePosition { get; set; }

        public string StatusMotoOne { get; set; }

        [DisplayName("Moto Two")]
        public string MotoTwoPosition { get; set; }
        
        public string StatusMotoTwo { get; set; }

        [DisplayName("Points")]
        public short Points { get; set; }

        [DisplayName("Points Total")]
        public short PointsTotal { get; set; }

        [DisplayName("Points Dropped")]
        public short PointsDropped { get; set; } 
        
        [DisplayName("Overall")]
        public short Overall { get; set; }

        [DisplayName("Note")]
        public string Note { get; set; }

    }
}
