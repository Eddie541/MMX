using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MMXModelsLib {
    public class MotorcycleModel : BaseModel {

        public int MotorcycleKey { get; set; }

        public int MemberKey { get; set; }

        [Required]
        [DisplayName("Brand")]
        public string Brand { get; set; }

        [DisplayName("Model")]
        public string CycleModel { get; set; }

        [DisplayName("Displacement")]
        public string Displacement { get; set; }

        [Required]
        [DisplayName("Riding Number")]
        public string RidingNumber { get; set; }

        [DisplayName("Year")]
        public short Year { get; set; }

        [DisplayName("Summary")]
        public string Summary {
            get { return Brand + " " + (string.IsNullOrEmpty(CycleModel) ? Displacement : CycleModel) + " " + RidingNumber; }
        }

    }
}
