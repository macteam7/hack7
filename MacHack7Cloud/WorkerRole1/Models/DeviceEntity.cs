using System;

namespace WorkerRole1.Models
{
    public class DeviceEntity : BaseEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public DeviceEntity()
        {
        }

        public override sealed void GenerateKeys()
        {
            PartitionKey = "Device";
            RowKey = Id;
        }
    }
}
