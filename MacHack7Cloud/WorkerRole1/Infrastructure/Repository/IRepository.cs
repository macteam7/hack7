using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace WorkerRole1.Infrastructure.Repository
{
    public interface IRepository<T> where T : TableEntity
    {
        IEnumerable<T> GetAll();

        IEnumerable<T> GetSpecificColumns(IList<string> columns);

        IEnumerable<T> GetAllInPartition(string partitionKey);

        IEnumerable<T> GetByAttribute(string nameAttribute, string valueAttribute);

        T GetEntity(string partitionKey, string rowKey);

        void Insert(T entity);

        void InsertBatch(IEnumerable<T> entities);

        void InsertOrReplace(T entity);

        void Delete(T entity);

        void DeleteByKey(string partitionKey, string rowKey);

        #region async operations
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllInPartitionAsync(string partitionKey);
        Task<TableResult> GetEntityAsync(string partitionKey, string rowKey);
        Task InsertAsync(T entity);
        Task InsertOrReplaceAsync(T entity);
        Task InsertBatchAsync(IEnumerable<T> entities);
        Task DeleteAsync(T entity);
        Task DeleteByKeyAsync(string partitionKey, string rowKey);
        #endregion
    }
}
