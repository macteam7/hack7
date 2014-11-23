using System;

namespace WorkerRole1.Models
{
    public class DevicePositions : BaseEntity
    {
        public DateTime Time { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string DeviceId { get; set; }

        public void SetKey(string key)
        {
            DeviceId = key;
            GenerateKeys();
        }

        public DevicePositions()
        {
            GenerateKeys();
        }

        public DevicePositions(string deviceId)
        {
            DeviceId = deviceId;
            GenerateKeys();
        }

        public override sealed void GenerateKeys()
        {
            PartitionKey = "DevicePositions";
            RowKey = DeviceId;
        }
    }
}
