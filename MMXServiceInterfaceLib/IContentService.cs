using System.Collections.Generic;
using System.Xml.Linq;

namespace MMXServiceInterfaceLib {
	public interface IContentService {
		string GetElementContent(string parent, string elementName);
        IEnumerable<XElement> GetElementContentCollection(string parent);
	}
}
