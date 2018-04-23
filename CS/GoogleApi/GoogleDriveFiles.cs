using System.Runtime.Serialization;

namespace GoogleApi {
    [DataContract]
    public class GoogleDriveFiles {
        [DataMember(Name = "files")]
        public GoogleFile[] Files { get; set; }
    }
}