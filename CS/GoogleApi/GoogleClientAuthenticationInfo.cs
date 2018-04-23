namespace GoogleApi {
    public class GoogleClientAuthenticationInfo : ClientAuthenticationInfo {
        public GoogleClientAuthenticationInfo() {
            Scope = "https://www.googleapis.com/auth/drive.readonly";
            ResponseType = "code";
            AccessType = "offline";
            State = "1";
        }
    }
}