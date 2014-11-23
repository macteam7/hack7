using System;

namespace WorkerRole1.Models
{
    public class AlertsEntity : BaseEntity
    {
        public Guid HospitalId { get; set; }
        public Guid PatientId { get; set; }
        public int Status { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string DeviceId { get; set; }

        private string _deviceId;

        public AlertsEntity()
        {
            GenerateKeys();
        }

        public void SetKey(string key)
        {
            _deviceId = key;
            GenerateKeys();
        }

        public AlertsEntity(string deviceId)
        {
            _deviceId = deviceId;
            GenerateKeys();
        }

        public override sealed void GenerateKeys()
        {
            PartitionKey = "Alert";
            RowKey = Guid.NewGuid().ToString();
        }
    }
}
