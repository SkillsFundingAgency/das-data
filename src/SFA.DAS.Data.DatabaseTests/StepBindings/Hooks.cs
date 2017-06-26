using TechTalk.SpecFlow;
using StructureMap;
using SFA.DAS.Data.Worker.DependencyResolution;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.DatabaseTests.TestHelpers;

namespace SFA.DAS.Data.DatabaseTests.StepBindings
{
    [Binding]
    public class Hooks
    {
        private string _connectionString = null;

        [BeforeScenario]
        public void BeforeScenario()
        {
            var container = new Container(c =>
            {
                c.AddRegistry<DefaultRegistry>();
            });

            _connectionString = container.GetInstance<IDataConfiguration>().DatabaseConnectionString;
            var dbHelper = new TruncateHelper(_connectionString);
            dbHelper.TruncateAllTable().Wait();
            ScenarioContext.Current.Set(_connectionString, "connectionstring");
        }
    }
}
