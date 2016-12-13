using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SFA.DAS.Data.Pipeline.Tests
{
    [TestClass]
    public class PipelineTests
    {
        public class TestMessage
        {
            public string Message { get; set; }
        }

        [TestMethod]
        public void SingleStageSuccess()
        {
            var m = new TestMessage { Message = "bob" };

            var result = m.Return()
                .Bind(x => Result.Win(
                   new TestMessage { Message = "hello " + x.Message },
                   "said hello"));

            Assert.IsInstanceOfType(result, typeof(Success<TestMessage>));
            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual("hello bob", result.Content.Message);
            Assert.AreEqual("Success: said hello", result.Messages.First());
        }

        [TestMethod]
        public void SingleStageFailure()
        {
            var m = new TestMessage { Message = "bob" };

            var result = m.Return()
                .Bind(x => Result.Fail("it go bang"));

            Assert.IsInstanceOfType(result, typeof(Failure<string>));
            Assert.IsFalse(result.IsSuccess());
            Assert.IsNull(result.Content);
            Assert.AreEqual("Failure: it go bang", result.Messages.First());
        }

        [TestMethod]
        public void SingleStageException()
        {
            var m = new TestMessage { Message = "bob" };

            var result = m.Return()
                .Bind(x =>
                {
                    throw new Exception("big bang");
                    return Result.Win(
                        new TestMessage {Message = "hello " + x.Message},
                        "said hello");
                });

            Assert.IsInstanceOfType(result, typeof(Failure<TestMessage>));
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual("Exception: big bang", result.Messages.First());
        }
    }
}
