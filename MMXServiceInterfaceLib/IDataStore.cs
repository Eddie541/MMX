using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMXServiceInterfaceLib {
    public interface IDataStore {
        object this[string key] { get; set; }
    }
}
