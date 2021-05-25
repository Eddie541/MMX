using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MMXModelsLib {
    public class RaceModel : BaseModel {

        public int RaceKey { get; set; }

        public int TrackKey { get; set; }

        [DisplayName("Track")]
        public string TrackName { get; set; }

        [DisplayName("City")]
        public string City { get; set; }

        public IEnumerable<TrackModel> Tracks { get; set; }

        [Required]
        [DisplayName("Race")]
        public string RaceName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Race Date")]
        public DateTime RaceDate { get; set; }




    }
}
