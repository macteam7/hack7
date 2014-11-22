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
        public IEnumerable<PatientEntity> Get()
        {
            return repository.GetAll();
        }

        [Route("add")]
        [HttpPost]
        public void AddPatient([FromBody] PatientEntity patient)
        {
            repository.Insert(patient);
        }

        [Route("remove")]
        [HttpPost]
        public void RemovePatient(string idPatient)
        {
            repository.DeleteByKey("Patient",idPatient);
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
