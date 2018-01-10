using System.Data;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.EmploymentCheck.Events;

namespace SFA.DAS.Data.Infrastructure.Data
{
    public class EmploymentCheckRepository : BaseRepository, IEmploymentCheckRepository
    {
        public EmploymentCheckRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task SaveEmploymentCheck(EmploymentCheckCompleteEvent @event)
        {
            await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@nationalInsuranceNumber", @event.NationalInsuranceNumber, DbType.String);
                parameters.Add("@uln", @event.Uln, DbType.Int64);
                parameters.Add("@employerAccountId", @event.EmployerAccountId, DbType.Int64);
                parameters.Add("@ukprn", @event.Ukprn, DbType.Int64);
                parameters.Add("@checkDate", @event.CheckDate, DbType.Date);
                parameters.Add("@checkPassed", @event.CheckPassed, DbType.Boolean);

                return await c.ExecuteAsync(
                    sql: "[Data_Load].[CreateEmploymentCheck]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            });
        }
    }
}
