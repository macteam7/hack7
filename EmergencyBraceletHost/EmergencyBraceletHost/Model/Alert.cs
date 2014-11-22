using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmergencyBraceletHost.Model
{
    public class Alert
    {
        public string DeviceID { get; set; }
        public string PatientID { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
    }
}
