using System.Runtime.Serialization;

namespace GoogleApi {
    [DataContract]
    public class ValidateTokenResponse {
        [DataMember(Name = "audience")]
        public string Audience { get; set; }

        [DataMember(Name = "scope")]
        public string Scope { get; set; }

        [DataMember(Name = "userid")]
        public string UserId { get; set; }

        [DataMember(Name = "expires_in")]
        public string ExpiresIn { get; set; }

        [DataMember(Name = "error")]
        public string Error { get; set; }
    }
}