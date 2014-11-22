using System;

namespace WorkerRole1.Models
{
    public class PatientEntity : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public uint Age { get; set; }
        public GenderType Gender { get; set; }
        public string History { get; set; }

        public PatientEntity()
        {
            GenerateKeys();
        }

        public override sealed void GenerateKeys()
        {
            PartitionKey = "Patient";
            RowKey = Id.ToString();
        }
    }
}
