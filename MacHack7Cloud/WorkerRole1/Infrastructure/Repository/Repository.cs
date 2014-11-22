using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace WorkerRole1.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : TableEntity, new()
    {
        private readonly CloudTable _cloudTable;

        public Repository(string tableName)
        {
            var account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("StorageConnectionString"));
            var tableClient = account.CreateCloudTableClient();

            _cloudTable = tableClient.GetTableReference(tableName);
            _cloudTable.CreateIfNotExists();
        }

        public IEnumerable<T> GetAll()
        {
            var query = new TableQuery<T>();

            return _cloudTable.ExecuteQuery(query);
        }

        public IEnumerable<T> GetSpecificColumns(IList<string> columns)
        {
            var projectionQuery = new TableQuery<T>().Select(columns);
            var x = _cloudTable.ExecuteQuery(projectionQuery);
            return _cloudTable.ExecuteQuery(projectionQuery);
        }

        public IEnumerable<T> GetAllInPartition(string partitionKey)
        {
            var query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));

            return _cloudTable.ExecuteQuery(query);
        }

        public IEnumerable<T> GetByAttribute(string nameAttribute, string valueAttribute)
        {
            var query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition(nameAttribute, QueryComparisons.Equal, valueAttribute));
            return _cloudTable.ExecuteQuery(query);
        }

        public T GetEntity(string partitionKey, string rowKey)
        {

            var query = TableOperation.Retrieve<T>(partitionKey, rowKey);
            var result = _cloudTable.Execute(query);

            return (T)result.Result;
        }

        public void Insert(T entity)
        {
            var insertOperation = TableOperation.Insert(entity);
            try
            {
                _cloudTable.Execute(insertOperation);
            }
            catch (StorageException exc)
            {
                Console.WriteLine(exc);
            }
        }

        public void InsertBatch(IEnumerable<T> entities)
        {
            var batchOperation = new TableBatchOperation();

            foreach (var entity in entities)
            {
                batchOperation.Insert(entity);
            }
            try
            {
                _cloudTable.ExecuteBatch(batchOperation);
            }
            catch (StorageException exc)
            {
                Console.WriteLine(exc);
            }
        }

        public void InsertOrReplace(T entity)
        {
            var insertOrReplaceOperation = TableOperation.InsertOrReplace(entity);
            try
            {
                _cloudTable.Execute(insertOrReplaceOperation);
            }
            catch (StorageException exc)
            {
                Console.WriteLine(exc);
            }
        }

        public void Delete(T entity)
        {

            var deleteOperation = TableOperation.Delete(entity);
            try
            {
                _cloudTable.Execute(deleteOperation);
            }
            catch (StorageException exc)
            {
                Console.WriteLine(exc);
            }
        }

        public void DeleteByKey(string partitionKey, string rowKey)
        {
            var entityToDelete = this.GetEntity(partitionKey, rowKey);
            var deleteOperation = TableOperation.Delete(entityToDelete);
            try
            {
                _cloudTable.Execute(deleteOperation);
            }
            catch (StorageException exc)
            {
                Console.WriteLine(exc);
            }
        }
        
        #region async operations

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var query = new TableQuery<T>();

            return await Task.Run(() => _cloudTable.ExecuteQuery(query));
        }

        public async Task<IEnumerable<T>> GetAllInPartitionAsync(string partitionKey)
        {

            var query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));

            return await Task.Run(() => _cloudTable.ExecuteQuery(query));

        }

        public async Task<TableResult> GetEntityAsync(string partitionKey, string rowKey)
        {
            var query = TableOperation.Retrieve<T>(partitionKey, rowKey);

            return await _cloudTable.ExecuteAsync(query);
        }

        public async Task InsertAsync(T entity)
        {
            var insertOperation = TableOperation.Insert(entity);

            try
            {
                await _cloudTable.ExecuteAsync(insertOperation);
            }
            catch (StorageException exc)
            {
                Console.WriteLine(exc);
            }
        }

        public async Task InsertOrReplaceAsync(T entity)
        {

            var insertOrReplaceOperation = TableOperation.InsertOrReplace(entity);
            try
            {
                await _cloudTable.ExecuteAsync(insertOrReplaceOperation);
            }
            catch (StorageException exc)
            {
                Console.WriteLine(exc);
            }
        }

        public async Task InsertBatchAsync(IEnumerable<T> entities)
        {

            var batchOperation = new TableBatchOperation();

            foreach (var entity in entities)
            {
                await Task.Run(() => batchOperation.Insert(entity));
            }
            try
            {
                await _cloudTable.ExecuteBatchAsync(batchOperation);
            }
            catch (StorageException exc)
            {
                Console.WriteLine(exc);
            }
        }

        public async Task DeleteAsync(T entity)
        {

            var deleteOperation = TableOperation.Delete(entity);
            try
            {
                await _cloudTable.ExecuteAsync(deleteOperation);
            }
            catch (StorageException exc)
            {
                Console.WriteLine(exc);
            }
        }

        public async Task DeleteByKeyAsync(string partitionKey, string rowKey)
        {
            var entityToDelete = this.GetEntity(partitionKey, rowKey);
            var deleteOperation = TableOperation.Delete(entityToDelete);
            try
            {
                await _cloudTable.ExecuteAsync(deleteOperation);
            }
            catch (StorageException exc)
            {
                Console.WriteLine(exc);
            }
        }

        #endregion
    }
}
