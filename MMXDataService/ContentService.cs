using ESS.UtilitiesLib;
using System.Collections.Generic;
using System.Xml.Linq;
using MMXServiceInterfaceLib;


namespace MMXDataService {
	public class ContentService : IContentService {

		XMLBuilder builder; 

		public ContentService(string contentFileLocation) {
			builder = new XMLBuilder(contentFileLocation, true);

		}


		#region IContentService Members

		public string GetElementContent(string parent, string elementName) {
			object o = builder.GetValueAt(parent, elementName);
			if (o != null) {
				return o.ToString();
			} else {
				return "";
			}
			 
		}


        public IEnumerable<XElement> GetElementContentCollection(string parent) {         
            return builder.GetFirstDecendants(parent);
        }

        #endregion
    }
}
