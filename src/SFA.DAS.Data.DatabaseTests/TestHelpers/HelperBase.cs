using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace SFA.DAS.Data.DatabaseTests.TestHelpers
{
    public abstract class HelperBase
    {
        protected readonly string _connectionString;
        public HelperBase(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected readonly List<string> tables = new List<string>
        {
            "[DAS_Employer_LegalEntities]",
            "[Das_Commitments]",
            "[DAS_Employer_Accounts]",
            "[DAS_Employer_Agreements]",
            "[DAS_Employer_LegalEntities]",
            "[DAS_Employer_PayeSchemes]",
            "[DAS_FailedEvents]",
            "[DAS_LevyDeclarations]",
            "[DAS_LoadedEvents]",
            "[DAS_Payments]"
        };

        protected async Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                return await getData(connection);
            }
        }
    }
}