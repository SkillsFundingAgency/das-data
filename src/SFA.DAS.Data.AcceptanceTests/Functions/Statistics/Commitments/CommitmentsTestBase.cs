using System;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SFA.DAS.Data.AcceptanceTests.Data;
using SFA.DAS.Data.AcceptanceTests.Data.DTOs;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Infrastructure.Data;
using SFA.DAS.Data.Infrastructure.Services;
using SFA.DAS.NLog.Logger;
using StructureMap;

namespace SFA.DAS.Data.AcceptanceTests.Functions.Statistics.Commitments
{
    public class CommitmentsTestBase
    {
        public StatisticsTestsRepository StatisticsTestsRepository;
        public ILog Log;
        public IMediator Mediator;
        public IStatisticsService StatisticsService;
        public IStatisticsRepository StatisticsRepository;
        public Mock<IEasStatisticsHandler> EasStatisticsHandlerMock;
        public Mock<ICommitmentsStatisticsHandler> CommitmentsStatisticsHandlerMock;
        public Mock<IPaymentStatisticsHandler> PaymentsStatisticsHandlerMock;

        public async Task SetupDatabase()
        {
            var container = new Container(c =>
            {
                c.AddRegistry<DAS.Data.Functions.Ioc.DefaultRegistry>();
            });

            Mediator = container.GetInstance<IMediator>();
            StatisticsRepository = container.GetInstance<IStatisticsRepository>();

            Log = new NLogLogger(typeof(StatisticsService), null);
            EasStatisticsHandlerMock = new Mock<IEasStatisticsHandler>();
            CommitmentsStatisticsHandlerMock = new Mock<ICommitmentsStatisticsHandler>();
            PaymentsStatisticsHandlerMock = new Mock<IPaymentStatisticsHandler>();

            StatisticsService = new StatisticsService(Log, EasStatisticsHandlerMock.Object,
                StatisticsRepository, Mediator,
                CommitmentsStatisticsHandlerMock.Object, PaymentsStatisticsHandlerMock.Object);

            StatisticsTestsRepository = new StatisticsTestsRepository(DataAcceptanceTests.Config.DatabaseConnectionString);
            await StatisticsTestsRepository.DeleteConsistencyChecks();
            await StatisticsTestsRepository.DeleteCommitments();
            await StatisticsTestsRepository.InsertCommitmentsData(new CommitmentsRecord
            {
                CommitmentID = 123, PaymentStatus = "Active", ApprenticeshipID = 123, AgreementStatus = "Agreed",
                ProviderID = "ABC", LearnerID = "ABC", EmployerAccountID = "ABC", TrainingTypeID = "ABC", TrainingID = "ABC",
                TrainingStartDate = DateTime.Now, TrainingEndDate = DateTime.Now, TrainingTotalCost = new decimal(123.12), UpdateDateTime = DateTime.Now,
                LegalEntityCode = "ABC", LegalEntityName = "ABC", LegalEntityOrganisationType = "ABC", DateOfBirth = DateTime.Now, IsLatest = true, TransferSenderAccountId = null,
                TransferApprovalDate = DateTime.Now, TransferApprovalStatus = "ABC"
            });
        }
    }
}
