using System;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.Tests.Builders
{
    public class PayeSchemeViewModelBuilder
    {
        private string _dasAccountId = "AHC12343";
        private string _name = "Name";
        private DateTime _removedDate = DateTime.Now;
        private DateTime _addedDate = DateTime.Now.AddYears(1);
        private string _ref = "payeref";

        public PayeSchemeViewModelBuilder WithDasAccountId(string dasAccountId)
        {
            _dasAccountId = dasAccountId;
            return this;
        }

        public PayeSchemeViewModelBuilder WithRef(string payeRef)
        {
            _ref = payeRef;
            return this;
        }

        public PayeSchemeViewModel Build()
        {
            return new PayeSchemeViewModel
            {
                DasAccountId = _dasAccountId,
                Name = _name,
                AddedDate = _addedDate,
                Ref = _ref,
                RemovedDate = _removedDate
            };
        }
    }
}
