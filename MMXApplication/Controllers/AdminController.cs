using System.Web.Mvc;
using MMXModelsLib;
using MMXServiceInterfaceLib;
using MMXDataService;

namespace MMXApplication.Controllers
{
    public class AdminController : BaseController {

       
              

        [HttpGet]
        [Authorize]
        public ActionResult ProcessRaceResults(string resultsFile) {
            //todo get the current years unsaved (has no results) races with the most recent past
            // selected
            ProcessRaceResultsModel model = new ProcessRaceResultsModel() {
                FileName = resultsFile
               
            };
          
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult ProcessRaceResults(ProcessRaceResultsModel model) {
            

            return View();
        }


    }
}
