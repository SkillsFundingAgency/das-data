using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Data.Domain.Models;

namespace SFA.DAS.Data.Application.UnitTests.Handlers.CommitmentsStatisticsHandler
{
    [TestFixture]
    public class WhenHandleMethodIsCalled : StatisticsHandlerBase<CommitmentsStatisticsModel>
    {
        private Application.Handlers.CommitmentsStatisticsHandler _handler;

        [SetUp]
        public void Setup()
        {
            base.SetUp(o => o.CommitmentsStatisticsEndPoint);

            _handler = new Application.Handlers.CommitmentsStatisticsHandler(HttpClientWrapper.Object, Configuration.Object, Logger.Object);
        }

        protected override async Task<dynamic> CallHandleMethodOnHandler()
        {
            return await _handler.Handle();
        }
    }
}
