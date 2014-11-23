using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using WorkerRole1.Infrastructure.Repository;
using WorkerRole1.Models;

namespace WorkerRole1
{
    [EnableCors(methods: "*", headers: "*", origins: "*")]
    [RoutePrefix("api/DevicePatient")]
    public class PatientDevicesController : ApiController
    {
        private readonly IRepository<PatientDevicesEntity> repository;

        public PatientDevicesController()
        {
            repository = new Repository<PatientDevicesEntity>("PatientDevicesEntity");
        }

        [Route("")]
        public IEnumerable<PatientDeviceModel> Get()
        {
            return repository.GetAll().Select(patientDevice => new PatientDeviceModel()
                                                                             {
                                                                                 DeviceId = patientDevice.DeviceId.ToString(),
                                                                                 PatientId = patientDevice.RowKey
                                                                             });
        }

        [Route("add")]
        [HttpPut]
        public bool AddDeviceToPatient([FromBody] PatientDeviceModel patientDevice)
        {
            Guid patientId;
            if (Guid.TryParse(patientDevice.PatientId, out patientId))
            {
                repository.Insert(new PatientDevicesEntity(patientId)
                                  {
                                      DeviceId = patientDevice.DeviceId
                                  });
                return true;
            }
            return false;
        }

        [Route("remove/{patientId}")]
        [HttpPost]
        public void RemoveDevice(string patientId)
        {
            var old = repository.GetEntity("PatientDevice", patientId);
            if (old != null) repository.Delete(old);
        }
    }

    public class PatientDeviceModel
    {
        public string DeviceId { get; set; }
        public string PatientId { get; set; }
    }
}
