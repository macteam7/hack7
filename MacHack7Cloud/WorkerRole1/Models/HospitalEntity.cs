using System;

namespace WorkerRole1.Models
{
    public class HospitalEntity : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public override void GenerateKeys()
        {
            PartitionKey = Id.ToString();
            RowKey = Guid.NewGuid().ToString();
        }
    }
}
