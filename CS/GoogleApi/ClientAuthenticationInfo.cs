using System.Xml.Linq;
using DevExpress.DataAccess.Native;
using DevExpress.Utils;

namespace GoogleApi {
    public class ClientAuthenticationInfo {
        const string XML_ClientID = "ClientID";
        const string XML_ClientSecret = "ClientSecret";
        const string XML_RedirectUri = "RedirectUri";
        const string XML_ResponseType = "ResponseType";
        const string XML_Scope = "Scope";
        const string XML_AccessType = "AccessType";
        const string XML_State = "State";

        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUri { get; set; }
        public string ResponseType { get; set; }
        public string Scope { get; set; }
        public string AccessType { get; set; }
        public string State { get; set; }

        protected internal void SaveToXml(XElement clientInfo) {
            Guard.ArgumentNotNull(clientInfo, "clientInfo");

            if(ClientID != null)
                clientInfo.Add(new XAttribute(XML_ClientID, ClientID));
            if(ClientSecret != null)
                clientInfo.Add(new XAttribute(XML_ClientSecret, ClientSecret));
            if(RedirectUri != null)
                clientInfo.Add(new XAttribute(XML_RedirectUri, RedirectUri));
            if(ResponseType != null)
                clientInfo.Add(new XAttribute(XML_ResponseType, ResponseType));
            if(Scope != null)
                clientInfo.Add(new XAttribute(XML_Scope, Scope));
            if(AccessType != null)
                clientInfo.Add(new XAttribute(XML_AccessType, AccessType));
            if(State != null)
                clientInfo.Add(new XAttribute(XML_State, State));
        }

        protected internal void LoadFromXml(XElement clientInfo) {
            Guard.ArgumentNotNull(clientInfo, "clientInfo");

            ClientID = clientInfo.GetAttributeValue(XML_ClientID);
            ClientSecret = clientInfo.GetAttributeValue(XML_ClientSecret);
            RedirectUri = clientInfo.GetAttributeValue(XML_RedirectUri);
            ResponseType = clientInfo.GetAttributeValue(XML_ResponseType);
            Scope = clientInfo.GetAttributeValue(XML_Scope);
            AccessType = clientInfo.GetAttributeValue(XML_AccessType);
            State = clientInfo.GetAttributeValue(XML_State);
        }
    }
}
