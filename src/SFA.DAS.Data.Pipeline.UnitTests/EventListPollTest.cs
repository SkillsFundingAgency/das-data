﻿using System.Collections.Generic;
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
            public LogToList Logger = new LogToList(); 
            public List<string> Output = new List<string>();

            public override void Configure(EntityListPoll<string, string> cfg)
            {
                cfg
                    .SetSource(() => new List<string> { "bob", "fred", "jim" })
                    .SetLog(Logger.Log)
                    .BuildPipeline(
                        r => r
                            .Step(s => Result.Win("Hi " + s, "not much")
                            .Step(s2 =>
                                {
                                    Output.Add(s2);
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

            Assert.AreEqual(3, job.Output.Count);
            Assert.AreEqual("Hi bob",job.Output.First());
        }
    }

    [TestFixture]
    public class EntityPollTest
    {
        public class One : EntityPoll<string, string>
        {
            public LogToList Logger = new LogToList();
            public List<string> Output = new List<string>();

            public override void Configure(EntityPoll<string, string> cfg)
            {
                cfg
                    .SetSource(() => "bob")
                    .SetLog(Logger.Log)
                    .BuildPipeline(
                        r => r
                            .Step(s => Result.Win("Hi " + s, "not much")
                            .Step(s2 =>
                            {
                                Output.Add(s2);
                                return Result.Win(s2, "stashed");
                            })));
            }
        }

        [Test]
        public void ProcessList()
        {
            var mock = new Mock<IJobExecutionContext>();
            var job = new One();
            job.Execute(mock.Object);

            Assert.AreEqual(1, job.Output.Count);
            Assert.AreEqual("Hi bob", job.Output.First());
        }
    }
}