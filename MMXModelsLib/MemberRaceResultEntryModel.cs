using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MMXModelsLib {
    public class MemberRaceResultEntryModel : BaseModel, IValidatableObject  {

        public int RaceResultKey { get; set; }

        public int RaceKey { get; set; }

        public int RaceClassMemberKey { get; set; }

        public int RaceClassKey { get; set; }

        public int EntryRow { get; set; }

        private bool entrySuccess = false;
        public bool EntrySuccess {
            get { return entrySuccess; }
            set { entrySuccess = value; }
        }

        [DisplayName("Last Name")]        
        public string LastName { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }     

        
        [DisplayName("Brand")]
        public string Brand { get; set; }

        
        [DisplayName("Riding Number")]  
        //[Required]
        public string RidingNumber { get; set; }

        
        [DisplayName("Moto One")]
        //[DataType("Int16")]
        //[Required]
        public short MotoOnePosition { get; set; }

        
        [DisplayName("Status Moto One")]
        public string StatusMotoOne { get; set; }

        
        [DisplayName("Moto Two")]
        //[DataType("Int16")]
        //[Required]
        public short MotoTwoPosition { get; set; }

        
        [DisplayName("Status Moto Two")]
        public string StatusMotoTwo { get; set; }

        
        [DisplayName("Overall")]
        //[DataType("Int16")]
        //[Required]
        public short Overall { get; set; }

        [DisplayName("Note")]
        public string Note { get; set; }


        public List<FinishSelection> FinishSelections {
            get {
                List<FinishSelection> fModel = new List<FinishSelection>();
                fModel.Add(new FinishSelection("Finished"));
                fModel.Add(new FinishSelection("DNF"));
                fModel.Add(new FinishSelection("DNS"));
                fModel.Add(new FinishSelection("DSQ"));
                return fModel;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            List<ValidationResult> results = new List<ValidationResult>();
            if (string.IsNullOrEmpty(this.Brand)) {
                results.Add(new ValidationResult("Brand cannot be empty", new string[] { "Brand" }));
            }
            if (string.IsNullOrEmpty(this.RidingNumber)) {
                results.Add(new ValidationResult("Missing riding number", new string[] { "RidingNumber" }));
            }
            if (this.MotoOnePosition < 1 && this.StatusMotoOne == "Finished") {
                results.Add(new ValidationResult("Moto one position cannot be zero unless set to DNF, DNS or DSQ", new string[] { "MotoOnePosition" }));                
            }
            if (this.MotoTwoPosition < 1 && this.StatusMotoTwo == "Finished") {
                results.Add(new ValidationResult("Moto two position cannot be zero unless set to DNF, DNS or DSQ", new string[] { "MotoTwoPosition" }));
            }
            if (this.Overall < 1) {
                results.Add(new ValidationResult("Overall finish invalid cannot be zero", new string[] { "Overall"}));               
            }

            return results;
        }
    }

    public class FinishSelection {
        public string Finish { get; private set; }
        public FinishSelection(string selection) {
            Finish = selection;
        }
    }

   
}
