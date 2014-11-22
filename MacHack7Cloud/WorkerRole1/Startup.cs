using Owin;
using System.Web.Http;
using WorkerRole1.Infrastructure.IOC;

namespace WorkerRole1
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            // Web API routes
            config.MapHttpAttributeRoutes();

            //Configure Unity IOC
            config.DependencyResolver = new UnityResolver(Bootstrapper.Initialise());

            //Enabling Cross-Origin Requests
            config.EnableCors();

            app.UseWebApi(config);
        }
    }
}