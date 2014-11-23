using System;

namespace WorkerRole1.Models
{
    public class PatientDevicesEntity : BaseEntity
    {
        public string DeviceId { get; set; }
        private Guid _patientId;

        public PatientDevicesEntity()
        {
            GenerateKeys();
        }

        public PatientDevicesEntity(Guid patientId)
        {
            _patientId = patientId;
            GenerateKeys();
        }

        public override sealed void GenerateKeys()
        {
            PartitionKey = "PatientDevice";
            RowKey = _patientId.ToString();
        }
    }
}
