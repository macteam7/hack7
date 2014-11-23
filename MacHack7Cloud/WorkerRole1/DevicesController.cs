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
    [RoutePrefix("api/devices")]
    public class DevicesController : ApiController
    {
        private readonly IRepository<DevicePositions> repository;
        private readonly IRepository<PatientEntity> patientRepository;
        private readonly IRepository<PatientDevicesEntity> patientDeviceRepository;
        private readonly IRepository<AlertsEntity> alertsRepository;

        public DevicesController()
        {
            repository = new Repository<DevicePositions>("DevicePositions");
            patientRepository = new Repository<PatientEntity>("PatientEntity");
            patientDeviceRepository = new Repository<PatientDevicesEntity>("PatientDevicesEntity");
            alertsRepository = new Repository<AlertsEntity>("AlertsEntity");
        }

        [Route("")]
        public IEnumerable<DevicePositionModel> Get()
        {
            return repository.GetAll().Select(device => new DevicePositionModel(device));
        }

        [Route("Extra")]
        public IEnumerable<DevicePositionExtraModel> GetExtra()
        {
            var listOfExtraDevices = new List<DevicePositionExtraModel>();

            var devicePositions = repository.GetAll();
            foreach (var devicePositionse in devicePositions)
            {
                var deviceExtraModel = new DevicePositionExtraModel(new DevicePositionModel(devicePositionse));

                var patientDevices = patientDeviceRepository.GetByAttribute("DeviceId", devicePositionse.DeviceId).ToList();
                if (patientDevices.Any())
                {
                    var patientDevicesEntity = patientDevices.FirstOrDefault();
                    if (patientDevicesEntity != null)
                    {
                        var patientId = patientDevicesEntity.RowKey;
                        var patient = patientRepository.GetEntity("Patient", patientId);
                        deviceExtraModel.Patient = patient;
                    }
                }

                var alerts = alertsRepository.GetByAttribute("DeviceId", devicePositionse.DeviceId);
                deviceExtraModel.Alerts = alerts;
                listOfExtraDevices.Add(deviceExtraModel);
            }
            return listOfExtraDevices;
        }
        
        [Route("")]
        [HttpPut]
        public void AddDevicePosition([FromBody] DevicePositions devicePosition)
        {
            devicePosition.Time = DateTime.UtcNow;
            devicePosition.GenerateKeys();

            var oldPos = repository.GetEntity("DevicePositions", devicePosition.DeviceId);
            if (oldPos != null)
            {
                repository.Delete(oldPos);
            }
            repository.Insert(devicePosition);
        }
    }
}
