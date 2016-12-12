using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SFA.DAS.Data.Pipeline.Tests
{
    [TestClass]
    public class ResultTests
    {
        public class TestMessage
        {
            public string Message { get; set; }
        }

        [TestMethod]
        public void SingleStage()
        {
            var m = new TestMessage { Message = "bob" };

            var result = m.Return()
                .Bind(x => Result.Win(
                   new TestMessage { Message = "hello " + x.Message },
                   "said hello"));

            Assert.IsInstanceOfType(result, typeof(Success<TestMessage>));
            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual("hello bob", result.Content.Message);
        }
    }
}
