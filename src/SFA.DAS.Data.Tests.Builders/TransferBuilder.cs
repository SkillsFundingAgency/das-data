using System;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Tests.Builders
{
    public class TransferBuilder
    {
        private static Random _idGenerator = new Random(0);
        private long _senderAccountId = 111;
        private long _receiverAccountId = 222;
        private Guid _requiredPaymentId = Guid.NewGuid();
        private decimal _amount = 123.45m;
        private string _type = "Levy";
        private long _commitmentId = 333;
        private string _collectionPeriodName = "PERIOD";

        public TransferBuilder WithPeriod(string period)
        {
            _collectionPeriodName = period;
            return this;
        }

        public AccountTransfer Build()
        {
            return new AccountTransfer
            {
                TransferId = _idGenerator.Next(int.MaxValue),
                Type = _type,
                SenderAccountId = _senderAccountId,
                ReceiverAccountId = _receiverAccountId,
                RequiredPaymentId = _requiredPaymentId,
                Amount = _amount,
                CommitmentId = _commitmentId,
                CollectionPeriodName = _collectionPeriodName
            };
        }
    }
}
