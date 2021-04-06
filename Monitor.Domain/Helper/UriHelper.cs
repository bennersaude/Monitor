using System.Text.RegularExpressions;

namespace Monitor.Domain.Helper
{
    public static class UriHelper
    {
        public static string RemoverApplicationPath(string urlPathAndQuery, string applicationPath)
        {
            var urlSemQueryParams = RemoverQueryParams(urlPathAndQuery);

            return !PossuiApplicationPath(applicationPath) ?
                urlSemQueryParams.TrimStart('/') :
                urlSemQueryParams.Replace(applicationPath, "").TrimStart('/');
        }

        public static string RemoverQueryParams(string url)
        {
            var regex = new Regex(@"(\/\d+$)");
            var match = regex.Match(url);
            return match.Success ?
                url.Replace(match.Groups[1].ToString(), "") :
                url;
        }

        public static bool IsService(string url)
        {
            return url.EndsWith(".svc");
        }

        private static bool PossuiApplicationPath(string applicationPath)
        {
            if (string.IsNullOrEmpty(applicationPath) || applicationPath.Equals("/"))
                return false;
            return true;
        }

        
    }
}
