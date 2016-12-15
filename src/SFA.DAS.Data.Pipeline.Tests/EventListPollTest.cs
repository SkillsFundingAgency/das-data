using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Quartz;
using Quartz.Impl;
using Quartz.Simpl;
using SFA.DAS.Data.Pipeline.Helpers;

namespace SFA.DAS.Data.Pipeline.Tests
{

    [TestClass]
    public class EventListPollTest
    {
        public class Some : EventListPoll<string, string>
        {
            public LogToList Log = new LogToList(); 
            public List<string> output = new List<string>();

            public override void Configure(EventListPoll<string, string> cfg)
            {
                cfg
                    .SetLog(Log.Log)
                    .BuildPipeline(
                        r => r
                            .Step(s => Result.Win("Hi " + s, "not much")
                            .Step(s2 =>
                                {
                                    output.Add(s2);
                                    return Result.Win(s2, "stashed");
                                }))
                            )
                    .SetSource(() => new List<string> {"bob","fred","jim"});
            }
        }

        [TestMethod]
        public void ProcessList()
        {
            var mock = new Mock<IJobExecutionContext>();
            var job = new Some();
            job.Execute(mock.Object);

            Assert.AreEqual(3, job.output.Count);
            Assert.AreEqual("Hi bob",job.output.First());
        }
    }
}