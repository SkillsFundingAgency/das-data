using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Quartz;
using SFA.DAS.Data.Pipeline.Helpers;

namespace SFA.DAS.Data.Pipeline.UnitTests
{

    [TestFixture]
    public class EventListPollTest
    {
        public class Some : EntityListPoll<string, string>
        {
            public LogToList Log = new LogToList(); 
            public List<string> output = new List<string>();

            public override void Configure(EntityListPoll<string, string> cfg)
            {
                cfg
                    .SetSource(() => new List<string> { "bob", "fred", "jim" })
                    .SetLog(Log.Log)
                    .BuildPipeline(
                        r => r
                            .Step(s => Result.Win("Hi " + s, "not much")
                            .Step(s2 =>
                                {
                                    output.Add(s2);
                                    return Result.Win(s2, "stashed");
                                }))
                            );
            }
        }

        [Test]
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