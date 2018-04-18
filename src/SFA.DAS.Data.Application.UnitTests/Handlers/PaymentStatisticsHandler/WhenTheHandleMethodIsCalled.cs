using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Data.Domain.Models;

namespace SFA.DAS.Data.Application.UnitTests.Handlers.PaymentStatisticsHandler
{
    [TestFixture]
    public class WhenTheHandleMethodIsCalled : StatisticsHandlerBase<PaymentStatisticsModel>
    {
        private Application.Handlers.PaymentsStatisticsHandler _handler;

        [SetUp]
        public void SetUpHandler()
        {
            base.SetUp(o => o.PaymentsStatisticsEndPoint);

            _handler = new Application.Handlers.PaymentsStatisticsHandler(HttpClientWrapper.Object, Configuration.Object, Logger.Object);
        }

        protected override async Task<dynamic> CallHandleMethodOnHandler()
        {
            return await _handler.Handle();
        }
    }
}
