using System;

namespace WorkerRole1.Models
{
    public class DevicePositions : BaseEntity
    {
        public DateTime Time { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public DevicePositions(Guid deviceId)
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
