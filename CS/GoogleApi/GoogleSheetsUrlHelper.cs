using System.Text.RegularExpressions;

namespace GoogleApi {
    public static class GoogleSheetsUrlHelper {
        public static string ExtractIdFromUrl(string url) {
            const string documentIdPattern = "(?<=/d/)[a-zA-Z0-9-_]+(?=/|$)";

            if(string.IsNullOrEmpty(url)) {
                return url;
            }

            url = url.Trim();

            var match = Regex.Match(url, documentIdPattern);
            if(match.Success) {
                return match.Groups[0].Value;
            }

            return null;
        }
    }
}
