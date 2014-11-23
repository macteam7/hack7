using System;

namespace WorkerRole1.Models
{
    public class AlertModel
    {
        private AlertsEntity _alertsEntity;

        public AlertModel(AlertsEntity alertsEntity)
        {
            _alertsEntity = alertsEntity;
        }

        public Guid HospitalId { get { return _alertsEntity.HospitalId; } }
        public Guid PatientId { get { return _alertsEntity.PatientId; } }
        public int Status { get { return _alertsEntity.Status; } }
        public double Latitude { get { return _alertsEntity.Latitude; } }
        public double Longitude { get { return _alertsEntity.Longitude; } }
        public string DeviceId { get { return _alertsEntity.DeviceId; } }
        public PatientEntity Patient { get; set; }
    }
}
