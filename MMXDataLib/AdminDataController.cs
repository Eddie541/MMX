using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using System.Configuration;
using System.Collections.Specialized;

namespace MMXDataLib {
    public class AdminDataController : BaseDataController {

                ////private int currentTake = 20;

        public AdminDataController() {
        }      

        public static object GetConfiguration(string key) {
            return GetApplicationConfigurationSettingValue(key);
        }

        private static object GetApplicationConfigurationSettingValue(string name) {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;

            string[] vals = appSettings.GetValues(name);

            if (vals.Count() > 0) {
                return vals[0];
            }
            else {
                return null;
            }
        }
    }
}
