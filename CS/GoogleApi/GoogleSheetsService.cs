using System;
using System.IO;
using System.Net.Http;

namespace GoogleApi {
    public class GoogleSheetsService : IDisposable {
        const string GoogleDriveEndPoint = "https://www.googleapis.com/drive/v3";
        const string GoogleDriveGetFile = "https://www.googleapis.com/drive/v3/files/{0}/export?mimeType=application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        const string AuthorizationHeaderName = "Authorization";

        readonly HttpClient client = new HttpClient();

        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        public ClientAuthenticationInfo ClientInfo { get; set; }

        public GoogleSheetsService(ClientAuthenticationInfo clientInfo, string accessToken) : this(clientInfo, accessToken, null) {
        }

        public GoogleSheetsService(ClientAuthenticationInfo clientInfo, string accessToken, string refreshToken) {
            this.RefreshToken = refreshToken;
            this.AccessToken = accessToken;
            this.ClientInfo = clientInfo;

            RefreshHeaders();
        }

        public GoogleDriveFiles ListFiles() {
            EnsureIsAuthorized();

            var httpResponseMessage = this.client.GetAsync(new Uri(GoogleDriveEndPoint + "/files")).Result;
            httpResponseMessage.EnsureSuccessStatusCode();
            var httpContent = httpResponseMessage.Content;
            string filesJson = httpContent.ReadAsStringAsync().Result;

            return JsonHelper.DeserealizeJson<GoogleDriveFiles>(filesJson);
        }

        public Stream GetFileStream(string documentId) {
            EnsureIsAuthorized();

            HttpResponseMessage httpResponseMessage = this.client.GetAsync(new Uri(string.Format(GoogleDriveGetFile, documentId))).Result;
            httpResponseMessage.EnsureSuccessStatusCode();

            return httpResponseMessage.Content.ReadAsStreamAsync().Result;
        }

        public void Authorize() {
            if(ClientInfo == null) {
                throw new Exception("need clientInfo");
            }

            AuthorizedResponse authorizedResponse;

            if(!string.IsNullOrEmpty(RefreshToken)) {
                authorizedResponse = GoogleOAuth.ExchangeRefreshToken(this.RefreshToken, this.ClientInfo);
            }
            else {
                string buildAuthUrl = GoogleOAuth.BuildAuthUrl(ClientInfo);
                string code = GoogleOAuth.Authenticate(buildAuthUrl, ClientInfo);
                authorizedResponse = GoogleOAuth.ExchangeCode(code, ClientInfo);
            }

            this.AccessToken = authorizedResponse.AccessToken;

            if(authorizedResponse.RefreshToken != null && RefreshToken != authorizedResponse.RefreshToken) {
                RefreshToken = authorizedResponse.RefreshToken;
            }

            RefreshHeaders();
        }

        #region IDisposable

        public void Dispose() {
            if(this.client != null) {
                this.client.Dispose();
            }
        }

        #endregion

        string AuthorizationHeaderValue { get { return "Bearer " + this.AccessToken; } }

        void EnsureIsAuthorized() {
            if(IsAuthorized()) {
                return;
            }

            Authorize();
        }

        bool IsAuthorized() {
            if(!string.IsNullOrEmpty(this.AccessToken)) {
                try {
                    ValidateTokenResponse validateTokenResponse = GoogleOAuth.ValidateToken(this.AccessToken);
                    return string.IsNullOrEmpty(validateTokenResponse.Error);
                }
                catch(HttpRequestException) {
                    return false;
                }
            }

            return false;
        }

        void RefreshHeaders() {
            this.client.DefaultRequestHeaders.Remove(AuthorizationHeaderName);
            this.client.DefaultRequestHeaders.Add(AuthorizationHeaderName, AuthorizationHeaderValue);
        }
    }
}
