using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace WISEsearch.MVC
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            config.Services.Replace(typeof (IAssembliesResolver), new WiseApiResolver());
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new {id = RouteParameter.Optional}
                );
        }
    }

    public class WiseApiResolver : IAssembliesResolver
    {
        public ICollection<Assembly> GetAssemblies()
        {
            return new[] {typeof (WISEsearch.Web.Api.Search.PersonSearchController).Assembly};
        }
    }
}
