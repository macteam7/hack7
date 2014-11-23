using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WorkerRole1.Infrastructure.Repository;
using WorkerRole1.Models;

namespace WorkerRole1
{
    [EnableCors(methods: "*", headers: "*", origins: "*")]
    [RoutePrefix("api/patients")]
    public class PatientController : ApiController
    {
        private readonly IRepository<PatientEntity> repository;

        public PatientController()
        {
            repository = new Repository<PatientEntity>("PatientEntity");
        }

        [Route("")]
        public IEnumerable<PatientModel> Get()
        {
            return repository.GetAll().Select(patient => new PatientModel(patient));
        }

        [Route("name/{name}")]
        public IEnumerable<PatientModel> GetByName(string name)
        {
            return repository.GetAll().Where(patient => patient.Name.StartsWith(name)).Select(patient => new PatientModel(patient));
        }

        [Route("id/{id}")]
        public PatientModel GetById(string id)
        {
            return new PatientModel(repository.GetEntity("Patient", id));
        }

        [Route("add")]
        [HttpPut]
        public Guid AddPatient([FromBody] PatientEntity patient)
        {
            patient.Id = Guid.NewGuid();
            patient.GenerateKeys();
            repository.Insert(patient);
            return patient.Id;
        }

        [Route("update")]
        [HttpPost]
        public bool UpdatePatient([FromBody] PatientEntity newPatient)
        {
            if (newPatient != null)
            {
                var oldPatient = repository.GetEntity("Patient", newPatient.Id.ToString());
                if (oldPatient != null) repository.Delete(oldPatient);
                newPatient.GenerateKeys();
                repository.Insert(newPatient);
                return true;
            }
            return false;
        }

        [Route("remove/{idPatient}")]
        [HttpPost]
        public void RemovePatient(Guid idPatient)
        {
            repository.DeleteByKey("Patient",idPatient.ToString());
        }

        [Route("removeAll")]
        [HttpGet]
        public void RemoveAll()
        {
            var patients = repository.GetAll();
            foreach (var patientEntity in patients)
            {
                repository.Delete(patientEntity);    
            }
        }

    }
}
