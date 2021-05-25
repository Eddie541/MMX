using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMXModelsLib {
    public class SuperUserDashboardModel : BaseModel {

        public int MemberKey { get; set; }

        public UploadRaceResultsViewModel FileUpload { get; set; }

        public EnrollmentModel Enrollment { get; set; }

        public short SelectedYear { get; set; }

        public SuperUserDashboardModel() {
            FileUpload = new UploadRaceResultsViewModel();
            Enrollment = new EnrollmentModel();

        }


    }
}
