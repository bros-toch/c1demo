using System.Web.Http;
using Composite.Core.Application;
using FluentValidation.WebApi;
using Models;

namespace Composite.AspNet.WebAPI
{
    [ApplicationStartup]
    public static class StartupHandler
    {
        public static void OnBeforeInitialize()
        {
        }

        public static void OnInitialized()
        {
            GlobalConfiguration.Configure(WebApiRegister);
            var formatters = GlobalConfiguration.Configuration.Formatters;

            formatters.Remove(formatters.XmlFormatter);
            //GlobalConfiguration.Configuration.Formatters.Insert(0, new CustomIDataXmlFormatter());
        }

        public static void WebApiRegister(HttpConfiguration config)
        {

            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });

            FluentValidationModelValidatorProvider.Configure(config);
            JsonIDataSerialization.WrapJsonContentResolver(config);
        }
    }
}
