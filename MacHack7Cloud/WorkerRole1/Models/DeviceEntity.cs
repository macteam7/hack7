using System;

namespace WorkerRole1.Models
{
    public class DeviceEntity : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public DeviceEntity()
        {
            GenerateKeys();
        }

        public override sealed void GenerateKeys()
        {
            PartitionKey = Id.ToString();
            RowKey = Guid.NewGuid().ToString();
        }
    }
}
