using System.Xml.Linq;
using DevExpress.Utils;

namespace GoogleApi {
    public static class XmlHelper {
        public static string GetAttributeValue(this XElement element, string attributeName) {
            Guard.ArgumentNotNull(element, "element");
            Guard.ArgumentIsNotNullOrEmpty(attributeName, "attributeName");
            XAttribute attribute = element.Attribute(attributeName);
            return attribute?.Value;
        }
    }
}