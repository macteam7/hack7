using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmergencyBraceletHost.Model
{
    public class Alert
    {
        public string RowKey { get; set; }
        public string DeviceId { get; set; }
        public string HospitalId { get; set; }
        public string PatientId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Status { get; set; }
    }
}
