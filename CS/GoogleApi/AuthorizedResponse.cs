using System.Runtime.Serialization;

namespace GoogleApi {
    [DataContract]
    public class AuthorizedResponse {
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }

        [DataMember(Name = "refresh_token")]
        public string RefreshToken { get; set; }

        [DataMember(Name = "expires_in")]
        public string ExpiresIn { get; set; }

        [DataMember(Name = "token_type")]
        public string TokenType { get; set; }
    }
}