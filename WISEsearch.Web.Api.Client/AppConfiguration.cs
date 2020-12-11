using System.Configuration;

namespace WISEsearch.Web.Api.Client
{
    public static class AppConfiguration
    {
        public static string GetWiseSearchBaseUrl()
        {
            var baseUrl = ConfigurationManager.AppSettings["WISEsearch.Api.BaseUrl"];
            if (baseUrl == null)
            {
                throw new ConfigurationErrorsException(
                    @"""WISEsearch.Api.BaseUrl"" was not set in the configuration file ");
            }

            return baseUrl;
        }
    }
}
