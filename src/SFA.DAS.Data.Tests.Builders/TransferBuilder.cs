using System;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Tests.Builders
{
    public class TransferBuilder
    {
        private Guid _id = Guid.NewGuid();
        private long _senderAccountId = 111;
        private long _receiverAccountId = 222;
        private Guid _requiredPaymentId = Guid.NewGuid();
        private decimal _amount = 123.45m;
        private string _type = "Levy";
        private DateTime _transferDate = DateTime.Today;
        private long _commitmentId = 333;
        private string _collectionPeriodName = "PERIOD";

        public TransferBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public TransferBuilder WithPeriod(string period)
        {
            _collectionPeriodName = period;
            return this;
        }

        public AccountTransfer Build()
        {
            return new AccountTransfer
            {
                Id = _id,
                Type = _type,
                SenderAccountId = _senderAccountId,
                ReceiverAccountId = _receiverAccountId,
                RequiredPaymentId = _requiredPaymentId,
                TransferDate = _transferDate,
                Amount = _amount,
                CommitmentId = _commitmentId,
                CollectionPeriodName = _collectionPeriodName
            };
        }
    }
}
