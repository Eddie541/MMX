using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MMXModelsLib {
    public class AddressModel : BaseModel {

        public int AddressKey { get; set; }

        [Required]
        [DisplayName("Street Address")]
        public string StreetAddress1 { get; set; }

        [DisplayName("Street Address")]
        public string StreetAddress2 { get; set; }

        [Required]
        [DisplayName("City")]
        public string City { get; set; }

        [Required]
        [DisplayName("State")]
        public string State { get; set; }

        [Required]
        [DisplayName("Zip Code")]
        public string ZipCode { get; set; }

        [DisplayName("Latitude")]
        [DisplayFormat(DataFormatString="{0:0.######}",
             ApplyFormatInEditMode = true, 
             NullDisplayText="")]            
        public decimal Latitude { get; set; }

        [DisplayName("Longitude")]
        [DisplayFormat(DataFormatString = "{0:0.######}",
             ApplyFormatInEditMode = true, 
             NullDisplayText="")]
        public decimal Longitude { get; set; }


    }
}
