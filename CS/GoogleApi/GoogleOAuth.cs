using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace GoogleApi {
    public static class GoogleOAuth {
        const string GoogleOAuthTokenInfoEndPoint = "https://www.googleapis.com/oauth2/v3/tokeninfo";
        const string GoogleOAuthTokenEndPoint = "https://accounts.google.com/o/oauth2/token";
        const string GoogleOAuthAuthEndPoint = "https://accounts.google.com/o/oauth2/auth";

        public static string BuildAuthUrl(ClientAuthenticationInfo info) {
            return string.Format("{6}?response_type={0}&client_id={1}&redirect_uri={2}&scope={3}&access_type={4}&state={5}",
                Uri.EscapeDataString(info.ResponseType),
                Uri.EscapeDataString(info.ClientID),
                Uri.EscapeDataString(info.RedirectUri),
                Uri.EscapeDataString(info.Scope),
                Uri.EscapeDataString(info.AccessType),
                Uri.EscapeDataString(info.State),
                GoogleOAuthAuthEndPoint);
        }

        public static string Authenticate(string authUrl, ClientAuthenticationInfo info) {
            string code = null;

            var startNew = Task.Factory.StartNew(() => {
                using(var httpListener = new HttpListener()) {
                    httpListener.Prefixes.Add(info.RedirectUri);

                    httpListener.Start();

                    while(true) {
                        var context = httpListener.GetContext();
                        var request = context.Request;
                        var result = request.QueryString;
                        if(result["state"] == info.State) {
                            code = result["code"];
                            return;
                        }
                    }
                }
            });

            Process.Start(authUrl);

            startNew.Wait(TimeSpan.FromSeconds(120));

            return code;
        }

        public static AuthorizedResponse ExchangeCode(string code, ClientAuthenticationInfo info) {
            var dictionary = new Dictionary<string, string> {
                { "client_id", info.ClientID },
                { "client_secret", info.ClientSecret },
                { "grant_type", "authorization_code" },
                { "redirect_uri", info.RedirectUri },
                { "code", code }
            };
            return PostToEndPoint<AuthorizedResponse>(dictionary, GoogleOAuthTokenEndPoint);
        }

        public static AuthorizedResponse ExchangeRefreshToken(string refreshToken, ClientAuthenticationInfo info) {
            var dictionary = new Dictionary<string, string> {
                { "refresh_token", refreshToken },
                { "client_id", info.ClientID },
                { "client_secret", info.ClientSecret },
                { "grant_type", "refresh_token" }
            };

            return PostToEndPoint<AuthorizedResponse>(dictionary, GoogleOAuthTokenEndPoint);
        }

        public static ValidateTokenResponse ValidateToken(string accessToken) {
            var dictionary = new Dictionary<string, string> {
                { "access_token", accessToken }
            };

            return PostToEndPoint<ValidateTokenResponse>(dictionary, GoogleOAuthTokenInfoEndPoint);
        }

        static T PostToEndPoint<T>(Dictionary<string, string> values, string endPoint) {
            string authorizedJson;

            using(var client = new HttpClient()) {
                var httpResponseMessage = client.PostAsync(endPoint, new FormUrlEncodedContent(values)).Result;
                httpResponseMessage.EnsureSuccessStatusCode();
                var httpContent = httpResponseMessage.Content;
                authorizedJson = httpContent.ReadAsStringAsync().Result;
            }

            return JsonHelper.DeserealizeJson<T>(authorizedJson);
        }
    }
}