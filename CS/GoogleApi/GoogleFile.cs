using System.Runtime.Serialization;

namespace GoogleApi {
    [DataContract]
    public class GoogleFile {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "mimeType")]
        public string MimeType { get; set; }
    }
}