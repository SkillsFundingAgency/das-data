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
        public void ThenIShouldGetAtleastRow(int rowCount)
        {
            var results = ScenarioContext.Current.Get<List<dynamic>>("viewresults");

            Assert.IsTrue(results.Count >= rowCount, string.Join(Environment.NewLine, results));
        }

        [Then(@"the view contains")]
        public void ThenTheViewContains(Table table)
        {
            var results = ScenarioContext.Current.Get<List<dynamic>>("viewresults");
            List<dynamic> values = table.CreateDynamicSet(false).ToList();
            Dictionary<int, bool> rowsfound = new Dictionary<int, bool>();
            int index = 1;
            foreach (var value in values)
            {
                var expected = (IDictionary<string, object>)value;
                rowsfound.Add(index, false);
                dynamic x = null;
                foreach (var result in results)
                {
                    var actual = (IDictionary<string, object>)result;
                    bool itemequal = false;
                    foreach (var i in expected)
                    {
                        object item;
                        object item1;
                        expected.TryGetValue(i.Key, out item);
                        actual.TryGetValue(i.Key, out item1);
                        System.Console.WriteLine($"Column:{i.Key} : expected:{item}-actual:{item1}");
                        itemequal = string.Equals(item.ToString(), item1.ToString(), StringComparison.OrdinalIgnoreCase);
                        if (itemequal == false) { break; }
                    }

                    if (itemequal == true)
                    {
                        rowsfound[index] = true;
                        x = result;
                        System.Console.WriteLine($"Row Found");
                        continue;
                    }
                    else
                    {
                        System.Console.WriteLine($"Row Not Found");
                    }
                }
                index++;
                if (x !=null) { results.Remove(x); }
            }
            Assert.IsTrue(rowsfound.Values.All(x => x == true), $"Rows {string.Join(Environment.NewLine, rowsfound.Where(x => x.Value == false).Select(y => y.Key))} did not match");
        }
    }
}
