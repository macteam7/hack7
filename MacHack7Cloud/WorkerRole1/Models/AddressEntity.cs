using System;

namespace WorkerRole1.Models
{
    public class AddressEntity : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string WifiSSID { get; set; }
        public string WiFiPWD { get; set; }

        public AddressEntity()
        {
            GenerateKeys();
        }

        public override sealed void GenerateKeys()
        {
            PartitionKey = "Address";
            RowKey = Id.ToString();
        }
    }
}
