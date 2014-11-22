using System.Net.Http.Headers;
using Owin;
using System.Web.Http;

namespace WorkerRole1
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config to return json
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            
            //Enabling Cross-Origin Requests
            config.EnableCors();

            app.UseWebApi(config);
        }
    }
}