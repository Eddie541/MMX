using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMXDataLib {
    public static class MMXDataContext {
        public static MMXEntities DataContext {
            get {
                if (DataStore.CurrentDataStore["MMXDataEntities"] == null) {
                    DataStore.CurrentDataStore["MMXDataEntities"] = new MMXEntities();
                }
                return (MMXEntities)DataStore.CurrentDataStore["MMXDataEntities"];
            }
        }
    }
}
