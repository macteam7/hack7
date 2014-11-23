using System;

namespace WorkerRole1.Models
{
    public class DevicePositionModel
    {
        private DevicePositions _devicePositions;

        public DevicePositionModel(DevicePositions devicePosition)
        {
            _devicePositions = devicePosition;
        }

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
            get { return _devicePositions.RowKey; }
        }

    }
}
