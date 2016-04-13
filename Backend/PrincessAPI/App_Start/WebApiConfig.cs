using System.Web.Http;
using PrincessAPI.Infrastructure;
using System.Data.Entity;

namespace PrincessAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            // Default Web API routes
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Set initializer for Entity Framework Database
            Database.SetInitializer<SystemDBContext>(
                new DropCreateDatabaseIfModelChanges<SystemDBContext>()
                );
        }
    }
}
