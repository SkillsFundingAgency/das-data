using SFA.DAS.EAS.Account.Api.Client;

namespace SFA.DAS.Data.AcceptanceTests
{
    public class AcceptanceTestConfiguration
    {
        public string DataConnectionString { get; set; }

        public AccountApiConfiguration AccountApiConfiguration { get; set; }
    }
}
