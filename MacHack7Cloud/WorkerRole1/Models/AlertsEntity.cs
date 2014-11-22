using System;

namespace WorkerRole1.Models
{
    public class AlertsEntity : BaseEntity
    {
        public Guid HospitalId { get; set; }
        public Status Status { get; set; }

        public AlertsEntity(Guid deviceId)
        {
            PartitionKey = deviceId.ToString();
            GenerateKeys();
        }

        public override sealed void GenerateKeys()
        {
            RowKey = Guid.NewGuid().ToString();
        }
    }
}
