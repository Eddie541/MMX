using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MMXModelsLib {
    public class MemberModel : BaseModel {

        public int MemberKey { get; set; }

        //[DisplayName("Member Name")]
        //public string UserName { get; set; }
        
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email address")]
        public string Email { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Middle Name")]
        public string MiddleName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Suffix")]
        public string Suffix { get; set; }

        [DataType(DataType.PhoneNumber)]
        [DisplayName("Phone")]
        public string Phone { get; set; }

        [DisplayName("License Number")]
        public string LicenseNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "DOB")]
        public DateTime DateOfBirth { get; set; }

        public int AddressKey { get; set; }

        public AddressModel Address { get; set; }
            
        [DisplayName("Last Activity")]
        [DataType(DataType.DateTime)]
        public DateTime? LastActivity { get; set; }

        [DisplayName("Last Login")]
        [DataType(DataType.DateTime)]
        public DateTime? LastLoginDate { get; set; }

        [DisplayName("Last Password Change")]
        [DataType(DataType.DateTime)]
        public DateTime? LastPasswordChangeDate { get; set; }

        public bool IsLocked { get; set; }
     
       
    }

   
}