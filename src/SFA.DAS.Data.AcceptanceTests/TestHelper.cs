using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Data.AcceptanceTests
{
    public class TestHelper
    {
        public static bool ConditionMet(Func<Task<bool>> callback, TimeSpan timeout)
        {
            bool conditionMet = false;

            var absoluteTimeout = DateTime.Now.Add(timeout);
            while (DateTime.Now < absoluteTimeout)
            {
                var callbackTask = callback();
                callbackTask.Wait();
                if (callbackTask.Result)
                {
                    conditionMet = true;
                    break;
                }
                Thread.Sleep(100);
            }

            return conditionMet;
        }
    }
}
