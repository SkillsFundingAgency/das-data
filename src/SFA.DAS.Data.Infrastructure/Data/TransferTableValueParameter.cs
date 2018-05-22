using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.SqlServer.Server;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Infrastructure.Data
{
    internal class TransferTableValueParameter : SqlMapper.IDynamicParameters
    {
        private readonly IEnumerable<AccountTransfer> _transfers;

        private static readonly SqlMetaData _transferIdMetaData = new SqlMetaData("TransferId", SqlDbType.BigInt);
        private static readonly SqlMetaData _senderAccountIdMetaData = new SqlMetaData("SenderAccountId", SqlDbType.BigInt);
        private static readonly SqlMetaData _receiverAccountIdMetaData = new SqlMetaData("ReceiverAccountId", SqlDbType.BigInt);
        private static readonly SqlMetaData _requiredPaymentId = new SqlMetaData("RequiredPaymentId", SqlDbType.UniqueIdentifier);
        private static readonly SqlMetaData _commitmentId = new SqlMetaData("CommitmentId", SqlDbType.BigInt);
        private static readonly SqlMetaData _amount = new SqlMetaData("Amount", SqlDbType.Decimal, 18, 5);
        private static readonly SqlMetaData _type = new SqlMetaData("Type", SqlDbType.NVarChar, 50);
        private static readonly SqlMetaData _collectionPeriodName = new SqlMetaData("CollectionPeriodName", SqlDbType.NVarChar, 10);

        public TransferTableValueParameter(IEnumerable<AccountTransfer> transfers)
        {
            _transfers = transfers;
        }

        public void AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            var sqlCommand = (SqlCommand) command;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            var items = new List<SqlDataRecord>();
            foreach (var param in _transfers)
            {
                var rec = new SqlDataRecord(_transferIdMetaData, _senderAccountIdMetaData, _receiverAccountIdMetaData, _requiredPaymentId, _commitmentId, _amount, _type, _collectionPeriodName);
                rec.SetInt64(0, param.TransferId);
                rec.SetInt64(1, param.SenderAccountId);
                rec.SetInt64(2, param.ReceiverAccountId);
                rec.SetGuid(3, param.RequiredPaymentId);
                rec.SetInt64(4, param.CommitmentId);
                rec.SetDecimal(5, param.Amount);
                rec.SetString(6, param.Type.ToString());
                rec.SetString(7, param.CollectionPeriodName);

                items.Add(rec);
            }

            var p = sqlCommand.Parameters.Add("@transfers", SqlDbType.Structured);
            p.Direction = ParameterDirection.Input;
            p.TypeName = "[Data_Load].[TransferEntity]";
            p.Value = items;
        }
    }
}