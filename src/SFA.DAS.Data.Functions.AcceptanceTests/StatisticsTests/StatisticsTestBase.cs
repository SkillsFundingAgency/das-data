using NUnit.Framework;

namespace SFA.DAS.Data.Functions.AcceptanceTests.StatisticsTests
{
    [TestFixture]
    public class StatisticsTestBase : FunctionEventTestBase
    {
        [Test]
        public void Test()
        {
            StartFunction("SFA.DAS.Data.Functions");
        }
    }
}
