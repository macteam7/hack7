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
            
            //Enabling Cross-Origin Requests
            config.EnableCors();

            app.UseWebApi(config);
        }
    }
}