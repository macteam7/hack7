using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.Practices.Unity;
using WorkerRole1.Infrastructure.IOC;
using WorkerRole1.Infrastructure.Repository;
using WorkerRole1.Models;

namespace WorkerRole1
{
    [EnableCors(methods: "*", headers: "*", origins: "*")]
    [RoutePrefix("api/test")]
    public class TestController : ApiController
    {
        private readonly IRepository<DeviceEntity> repository;

        public TestController()
        {
            repository = Bootstrapper.Container.Resolve(typeof(IRepository<DeviceEntity>), "Repository", new ParameterOverride("tableName", "devices")) as IRepository<DeviceEntity>;
            repository.Insert(new DeviceEntity());
        }

        [Route("")]
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent(String.Format("Hello from OWIN! (id = {0})", repository.GetAll().Count()))
            };
        }

        [Route("{id}")]
        public HttpResponseMessage Get(int id)
        {
            string msg = String.Format("Hello from OWIN (id = {0})", id);
            return new HttpResponseMessage()
            {
                Content = new StringContent(msg)
            };
        }
    }
}