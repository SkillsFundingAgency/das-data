﻿using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Infrastructure.Data
{
    public class PaymentRepository : BaseRepository, IPaymentRepository
    {
        public PaymentRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task SavePayments(IEnumerable<Payment> payments)
        {
            await WithConnection(async c =>
            {
                foreach (var payment in payments)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@PaymentId", payment.Id, DbType.String);
                    parameters.Add("@UkPrn", payment.Ukprn, DbType.Int64);
                    parameters.Add("@Uln", payment.Uln, DbType.Int64);
                    parameters.Add("@EmployerAccountId", payment.EmployerAccountId, DbType.String);
                    parameters.Add("@ApprenticeshipId", payment.ApprenticeshipId, DbType.Int64);
                    parameters.Add("@DeliveryMonth", payment.DeliveryPeriod.Month, DbType.Int32);
                    parameters.Add("@DeliveryYear", payment.DeliveryPeriod.Year, DbType.Int32);
                    parameters.Add("@CollectionMonth", payment.CollectionPeriod.Month, DbType.Int32);
                    parameters.Add("@CollectionYear", payment.CollectionPeriod.Year, DbType.Int32);
                    parameters.Add("@EvidenceSubmittedOn", payment.EvidenceSubmittedOn, DbType.DateTime);
                    parameters.Add("@EmployerAccountVersion", payment.EmployerAccountVersion, DbType.String);
                    parameters.Add("@ApprenticeshipVersion", payment.ApprenticeshipVersion, DbType.String);
                    parameters.Add("@FundingSource", payment.FundingSource.ToString(), DbType.String);
                    parameters.Add("@TransactionType", payment.TransactionType.ToString(), DbType.String);
                    parameters.Add("@Amount", payment.Amount, DbType.Decimal);
                    parameters.Add("@StandardCode", payment.StandardCode, DbType.Int64);
                    parameters.Add("@FrameworkCode", payment.FrameworkCode, DbType.Int32);
                    parameters.Add("@ProgrammeType", payment.ProgrammeType, DbType.Int32);
                    parameters.Add("@PathwayCode", payment.PathwayCode, DbType.Int32);
                    parameters.Add("@ContractType", payment.ContractType.ToString(), DbType.String);
                    parameters.Add("@CollectionPeriodName", payment.CollectionPeriod.Id, DbType.String);
                    var collectionPeriod = payment.CollectionPeriod.Id.Split('-');
                    if (collectionPeriod.Any() && collectionPeriod.Length == 2)
                    {
                        parameters.Add("@CollectionPeriodMonth", collectionPeriod[1], DbType.String);
                        parameters.Add("@CollectionPeriodYear", collectionPeriod[0], DbType.String);
                    }

                    await c.ExecuteAsync(
                        sql: "[Data_Load].[SavePayment]",
                        param: parameters,
                        commandType: CommandType.StoredProcedure);
                }
                return 0;
            });
        }

    }
}

