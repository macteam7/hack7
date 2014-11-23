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
    [RoutePrefix("api/Alerts")]
    public class AlertController : ApiController
    {
        private readonly IRepository<AlertsEntity> repository;
        private readonly IRepository<PatientEntity> patientRepository;

        public AlertController()
        {
            repository = new Repository<AlertsEntity>("AlertsEntity");
            patientRepository = new Repository<PatientEntity>("PatientEntity");
        }

        [Route("")]
        public IEnumerable<AlertsEntity> Get()
        {
            return repository.GetAll();
        }

        [Route("AcceptedAlert")]
        public IEnumerable<AlertsEntity> GetAcceptedAlerts()
        {
            return repository.GetAll().Where(alert => alert.Status == 2);
        }

        [Route("Extra")]
        public IEnumerable<AlertModel> GetExtra()
        {
            var listOfAlerts = new List<AlertModel>();

            var allerts = repository.GetAll();
            foreach (var alertsEntity in allerts)
            {
                var alertModel = new AlertModel(alertsEntity);
                alertModel.Patient = patientRepository.GetEntity("Patient", alertsEntity.PatientId.ToString());
                listOfAlerts.Add(alertModel);
            }

            return listOfAlerts;
        }

        [Route("add/{id}")]
        [HttpPut]
        public void AddAlert(string id, [FromBody] AlertsEntity alertEntity)
        {
            alertEntity.SetKey(id);
            repository.Insert(alertEntity);
        }

        [Route("update/{id}")]
        [HttpPost]
        public bool UpdateAlert(string id, [FromBody] AlertsEntity newAlertEntity)
        {
            var oldAlert = repository.GetEntity("Alert", id);
            if (oldAlert != null)
            {
                repository.Delete(oldAlert);
                newAlertEntity.SetKey(id);
                repository.Insert(newAlertEntity);
                return true;
            }
            return false;
        }
    }
}
