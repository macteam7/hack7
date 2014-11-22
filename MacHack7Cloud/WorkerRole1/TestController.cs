using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
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
            repository = new Repository<DeviceEntity>("DeviceEntity");
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