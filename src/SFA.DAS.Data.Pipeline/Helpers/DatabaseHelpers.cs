using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace SFA.DAS.Data.Pipeline.Helpers
{
    public class DbWrapper
    {
        public dynamic Wrapper { get; set; }
    }

    public static class DatabaseExtensions
    {
        public static PipelineResult<T> Store<T>(
            this PipelineResult<T> result, DbWrapper dbWrapper, string tableName)
        {
            return result.Step(r =>
            {
                dbWrapper.Wrapper[tableName].Insert(r);
                return Result.Win(r, "Inserted record into " + tableName);
            });
        }
    }

    public static class CloudStorageExtensions
    {
        public static PipelineResult<T> Store<T>(
            this PipelineResult<T> result, CloudStorageAccount storageAccount, string tableName) where T : TableEntity
        {
            return result.Step(r =>
            {
                var tableClient = storageAccount.CreateCloudTableClient();
                var table = tableClient.GetTableReference(tableName);
                table.CreateIfNotExists();
                var insertOperation = TableOperation.Insert(r);
                table.Execute(insertOperation);
                return Result.Win(r, "Inserted record into cloud storage " + tableName);
            });
        }
    }
}
