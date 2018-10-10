using System.Threading.Tasks;
using MediatR;
using Moq;
using SFA.DAS.Data.AcceptanceTests.Data;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Infrastructure.Services;
using SFA.DAS.NLog.Logger;
using StructureMap;

namespace SFA.DAS.Data.AcceptanceTests.Functions.Psrs
{
    public class PsrsTestBase
    {
        public ILog Log;
        public IMediator Mediator;
        public PsrsTestsRepository PsrsTestsRepository;
        public IPsrsRepository PsrsRepository;
        public IPsrsReportsService PsrsReportsService;
        public Mock<IPsrsExternalRepository> PsrsExternalRepositoryMock = new Mock<IPsrsExternalRepository>();
        
        public async Task SetupDatabase()
        {
            var container = new Container(c => { c.AddRegistry<DAS.Data.Functions.Ioc.DefaultRegistry>(); });

            Mediator = container.GetInstance<IMediator>();
            PsrsRepository = container.GetInstance<IPsrsRepository>();

            Log = new NLogLogger(typeof(StatisticsService), null);

            PsrsTestsRepository = new PsrsTestsRepository(DataAcceptanceTests.Config.DatabaseConnectionString);
            PsrsExternalRepositoryMock = new Mock<IPsrsExternalRepository>();

            await PsrsTestsRepository.DeletePublicSectorReports();
            await PsrsTestsRepository.DeletePublicSectorSummary();

            PsrsReportsService = new PsrsReportsService(PsrsExternalRepositoryMock.Object, PsrsRepository);
        }
    }
}
