
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
}
