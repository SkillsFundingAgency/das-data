using System.Web.Http;
using Owin;

namespace SFA.DAS.Data.IntegrationTests.ApiSubstitute
{
    public class ApiStartup
    {
        public void Configuration(IAppBuilder appBuilder, ApiSubstituteMessageHandler messageHandler)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.MessageHandlers.Add(messageHandler);

            appBuilder.UseWebApi(config);
        }
    }
}
