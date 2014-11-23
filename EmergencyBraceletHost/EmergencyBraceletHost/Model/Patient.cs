using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmergencyBraceletHost.Model
{
    public class Patient
    {
        public string Id { get; set; }
        public double LowTemp { get; set; }
        public double HighTemp { get; set; }
    }
}
