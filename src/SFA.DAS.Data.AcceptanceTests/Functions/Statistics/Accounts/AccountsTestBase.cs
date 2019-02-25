using System;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SFA.DAS.Data.AcceptanceTests.Data;
using SFA.DAS.Data.AcceptanceTests.Data.DTOs;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Infrastructure.Services;
using SFA.DAS.NLog.Logger;
using StructureMap;

namespace SFA.DAS.Data.AcceptanceTests.Functions.Statistics.Accounts
{
    public class AccountsTestBase
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
            await StatisticsTestsRepository.DeleteEmployerAccounts();
            await StatisticsTestsRepository.DeleteEmployerAgreements();
            await StatisticsTestsRepository.DeleteEmployerLegalEntities();
            await StatisticsTestsRepository.DeletePayments();
            await StatisticsTestsRepository.DeleteEmployerPayeSchemes();

            await StatisticsTestsRepository.InsertPaymentsData(new PaymentsRecord
            {
                CollectionYear = DateTime.Now.Year,  //Hima - Changed from 2018 to DateTime.Now.Year
                UpdateDateTime = DateTime.Now
            });

            await StatisticsTestsRepository.InsertEmployerPayeSchemes(new EmployerPayeSchemesRecord()
            {
                DasAccountId = "123", Ref = "123", AddedDate = DateTime.Now, UpdateDateTime = DateTime.Now, IsLatest = true
            });
            
            await StatisticsTestsRepository.InsertEmployerAccountsData(new EmployerAccountsRecord
            {
                DasAccountId = "123", AccountName = "abc", DateRegistered = DateTime.Now, OwnerEmail = "memecom",UpdateDateTime = DateTime.Now, AccountId = 123, IsLatest = true
            });
            await StatisticsTestsRepository.InsertEmployerLegalEntities(new EmployerLegalEntitiesRecord
            {
                DasAccountId = "123", DasLegalEntityId = 123, Status = "active", UpdateDateTime = DateTime.Now, IsLatest = true, Name = "abc", Source = "abc"
            });
            await StatisticsTestsRepository.InsertEmployerAgreements(new EmployerAgreementsRecord()
            {
                DasAccountId = "123", Status = "signed", DasLegalEntityId = 123, DasAgreementId = "123", UpdateDateTime = DateTime.Now, IsLatest = true
            });
        }
    }
}
