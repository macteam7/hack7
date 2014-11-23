using System;
using System.Collections.Generic;

namespace WorkerRole1.Models
{
    public class DevicePositionExtraModel
    {
        private DevicePositionModel _devicePositions;

        public DevicePositionExtraModel(DevicePositionModel devicePositionse)
        {
            _devicePositions = devicePositionse;
        }

        public PatientEntity Patient { get; set; }
        public IEnumerable<AlertsEntity> Alerts { get; set; }

        public DateTime Time
        {
            get { return _devicePositions.Time; }
        }

        public string Latitude
        {
            get { return _devicePositions.Latitude.ToString(); }
        }

        public string Longitude
        {
            get { return _devicePositions.Longitude.ToString(); }
        }

        public string DeviceId
        {
            get { return _devicePositions.DeviceId; }
        }
    }
}
