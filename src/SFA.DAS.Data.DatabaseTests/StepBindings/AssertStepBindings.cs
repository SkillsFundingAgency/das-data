using BoDi;
using TechTalk.SpecFlow;
using NUnit.Framework;
using System.Collections.Generic;
using System;
using TechTalk.SpecFlow.Assist;
using System.Linq;

namespace SFA.DAS.Data.DatabaseTests.StepBindings
{
    [Binding]
    public class AssertStepBindings
    {       
        [Then(@"I should get atleast (\d+) row")]
        public void ThenIShouldGetAtleastRow(int p0)
        {
            var results = ScenarioContext.Current.Get<List<object>>("viewresults");

            Assert.IsTrue(results.Count >= 1, string.Join(Environment.NewLine, results));
        }
    }
}
