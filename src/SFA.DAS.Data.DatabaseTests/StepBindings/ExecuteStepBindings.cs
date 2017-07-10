using BoDi;
using TechTalk.SpecFlow;
using SFA.DAS.Data.DatabaseTests.TestHelpers;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Data.DatabaseTests.StepBindings
{
    [Binding]
    public class ExecuteStepBindings
    {
        private ExecuteHelper dbHelper;
        
        public ExecuteStepBindings(IObjectContainer objectContainer)
        {
            dbHelper = new ExecuteHelper(ScenarioContext.Current.Get<string>("connectionstring"));
        }

        [When(@"I execute View (.*)")]
        public void WhenIExecuteViewReporting_LevyDetails(string view)
        {
            var asynccall = dbHelper.ExecuteView(view);
            asynccall.Wait();
            List<dynamic> results = asynccall.Result.ToList();
            ScenarioContext.Current.Set(results, "viewresults");
        }
    }
}
