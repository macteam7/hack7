using System;

namespace WorkerRole1.Models
{
    public class AddressEntity : BaseEntity
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public AddressEntity()
        {
            GenerateKeys();
        }

        public override sealed void GenerateKeys()
        {
            PartitionKey = Latitude.ToString() + Longitude;
            RowKey = Guid.NewGuid().ToString();
        }
    }
}
