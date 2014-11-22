using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace WorkerRole1.Models
{
    public abstract class BaseEntity : TableEntity, IBaseEntity
    {
        public abstract void GenerateKeys();

        // override object.Equals
        public override bool Equals(object obj)
        {
            BaseEntity entity = obj as BaseEntity;
            if (entity == null || obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            if (entity.PartitionKey.Equals(this.PartitionKey) && entity.RowKey.Equals(this.RowKey))
                return true;

            return base.Equals(obj);
        }
    }
}
