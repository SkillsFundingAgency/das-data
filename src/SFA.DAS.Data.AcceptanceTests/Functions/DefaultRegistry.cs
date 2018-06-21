using System.Linq;
using MediatR;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Infrastructure.Data;
using StructureMap;

namespace SFA.DAS.Data.AcceptanceTests.Functions
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            Scan(scan =>
            {
                var assemblyNames = (typeof(DefaultRegistry).Assembly.GetReferencedAssemblies()).ToList().Where(w => w.FullName.StartsWith("SFA.DAS.")).Select(a => a.FullName);

                foreach (var assemblyName in assemblyNames)
                {
                    scan.Assembly(assemblyName);
                }

                scan.RegisterConcreteTypesAgainstTheFirstInterface();
            });

            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));
            For<IMediator>().Use<Mediator>();

            For<IStatisticsRepository>().Use<StatisticsRepository>().Ctor<string>().Is(DataAcceptanceTests.Config.DatabaseConnectionString);
        }
    }
}
