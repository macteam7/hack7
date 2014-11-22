using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using WorkerRole1.Infrastructure.Repository;
using WorkerRole1.Models;

namespace WorkerRole1
{
    [EnableCors(methods: "*", headers: "*", origins: "*")]
    [RoutePrefix("api/locations")]
    public class LocationController : ApiController
    {
        private readonly IRepository<AddressEntity> repository;

        public LocationController()
        {
            repository = new Repository<AddressEntity>("AddressEntity");
        }

        [Route("")]
        public IEnumerable<AddressEntity> Get()
        {
            return repository.GetAll();
        }

        [Route("{id}")]
        public AddressEntity GetId(string id)
        {
            return repository.GetEntity("Address", id);    
        }

        [Route("add")]
        [HttpPost]
        public bool AddLocation([FromBody] AddressEntity location)
        {
            location.Id = Guid.NewGuid();
            repository.Insert(location);
            return true;
        }

        [Route("update")]
        [HttpPost]
        public bool UpdateLocation([FromBody] AddressEntity locationToUpdate)
        {
            if (locationToUpdate == null || Guid.Empty == locationToUpdate.Id)
            {
                return false;
            }
            var oldEntity = repository.GetEntity("Address", locationToUpdate.Id.ToString());
            if (oldEntity != null) repository.Delete(oldEntity);
            repository.Insert(locationToUpdate);
            return true;
        }
    }
}
