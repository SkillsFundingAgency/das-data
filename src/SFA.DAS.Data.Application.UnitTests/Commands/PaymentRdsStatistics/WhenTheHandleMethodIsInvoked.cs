using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Functions.Commands;
using SFA.DAS.Data.Functions.Commands.PaymentRdsStatistics;

namespace SFA.DAS.Data.Application.UnitTests.Commands.PaymentRdsStatistics
{
    [TestFixture]
    public class WhenTheHandleMethodIsInvoked :
        FunctionsCommandHandlerBase<PaymentStatisticsModel, RdsStatisticsForPaymentsModel, PaymentRdsStatisticsCommand>
    {
        private PaymentRdsStatisticsCommandHandler _handler;

        protected override void InitialiseHandler()
        {
            _handler = new PaymentRdsStatisticsCommandHandler(Repository.Object, Log.Object);
        }

        protected override Expression<Func<IStatisticsRepository, Task>> RepositoryExpression()
        {
            return o =>
                o.SavePaymentStatistics(It.IsAny<PaymentStatisticsModel>(), It.IsAny<RdsStatisticsForPaymentsModel>());
        }

        protected override PaymentRdsStatisticsCommand StatisticsCommand()
        {
            return new PaymentRdsStatisticsCommand
            {
                RdsStatisticsModel = new RdsStatisticsForPaymentsModel(),
                ExternalStatisticsModel = new PaymentStatisticsModel()
            };
        }

        protected override async Task<ICommandResponse> InvokeTheHandler(PaymentRdsStatisticsCommand command)
        {
            return await _handler.Handle(command);
        }
    }
}
