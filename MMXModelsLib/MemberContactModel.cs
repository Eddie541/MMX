using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MMXModelsLib {
    public class MemberContactModel : BaseModel {

        public int MemberContactKey { get; set; }

        public int AddressKey { get; set; }

        public AddressModel Address { get; set; }

        public int MemberKey { get; set; }

        public MemberModel Member { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Middle Name")]
        public string MiddleName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.PhoneNumber)]
        [DisplayName("Phone")]
        public string Phone { get; set; }

        [DisplayName("AMA Number")]
        public string AMANumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "DOB")]
        public DateTime DateOfBirth { get; set; }
        



    }
}
