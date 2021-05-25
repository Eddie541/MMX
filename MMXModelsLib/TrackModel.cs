using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MMXModelsLib {
    public class TrackModel : BaseModel {

        public int TrackKey { get; set; }
        public int AddressKey { get; set; }

        [Required]
        [DisplayName("Track")]
        public string TrackName { get; set; }

        [DisplayName("Contact")]
        public string ContactName { get; set; }

        [DataType(DataType.PhoneNumber)]
        [DisplayName("Phone")]
        public string Phone { get; set; }

        [DataType(DataType.Url)]
        [DisplayName("Web Address")]
        public string WebAddress { get; set; }

        [DataType(DataType.EmailAddress)]
        [DisplayName("Email Address")]
        public string EmailAddress { get; set; }
       
        public bool IsOpen { get; set; }

        public AddressModel Address { get; set; }
        
    }
}
