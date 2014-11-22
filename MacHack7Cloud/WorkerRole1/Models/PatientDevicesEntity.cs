using System;

namespace WorkerRole1.Models
{
    public class PatientDevicesEntity : BaseEntity
    {
        public Guid DeviceId { get; set; }

        public PatientDevicesEntity()
        {
            PartitionKey = Guid.NewGuid().ToString();
            GenerateKeys();
        }

        public PatientDevicesEntity(Guid patientId)
        {
            PartitionKey = patientId.ToString();
            GenerateKeys();
        }

        public override sealed void GenerateKeys()
        {
            RowKey = Guid.NewGuid().ToString();
        }
    }
}
