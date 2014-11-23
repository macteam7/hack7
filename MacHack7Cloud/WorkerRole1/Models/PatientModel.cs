using System;

namespace WorkerRole1.Models
{
    public class PatientModel
    {
        private PatientEntity _patientEntity;

        public PatientModel(PatientEntity patientEntity)
        {
            _patientEntity = patientEntity;
        }

        public Guid Id
        {
            get { return _patientEntity.Id; }
        }

        public string Name
        {
            get { return _patientEntity.Name; }
        }

        public string Age
        {
            get { return _patientEntity.Age.ToString(); }
        }

        public string Gender
        {
            get { return _patientEntity.Gender.ToString(); }
        }

        public string History
        {
            get { return _patientEntity.History; }
        }

        public string LowTemp
        {
            get { return _patientEntity.LowTemp.ToString(); }
        }

        public string HighTemp
        {
            get { return _patientEntity.HighTemp.ToString(); }
        }

        public string PartitionKey
        {
            get { return _patientEntity.PartitionKey; }
        }

        public string RowKey
        {
            get { return _patientEntity.RowKey; }
        }
    }
}
