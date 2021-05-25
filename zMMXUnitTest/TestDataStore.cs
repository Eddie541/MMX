using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMXServiceInterfaceLib;
using System.Web;
using MMXDataLib;

namespace zMMXUnitTest {    

    public class TestDataStore : IDataStore {
        public static IDataStore CurrentDataStore;

        public object this[string key] {
            get {
                return CurrentTestContext.Items[key];
            }
            set {
                CurrentTestContext.Items[key] = value;
            }
        }

        private static HttpContext CurrentTestContext {
            get { return new HttpContext(new HttpRequest(null, "http://tempuri.org", null), new HttpResponse(null)); }
        }

        public static string Connection = "metadata=res://*/MMXDataModel.csdl|res://*/MMXDataModel.ssdl|res://*/MMXDataModel.msl;provider=System.Data.SqlClient;provider connection string=\"data source=D6MW6641\\ESSDBSERVER;initial catalog=MMX;integrated security=True;multipleactiveresultsets=True;App=EntityFramework\"";
    }
}
