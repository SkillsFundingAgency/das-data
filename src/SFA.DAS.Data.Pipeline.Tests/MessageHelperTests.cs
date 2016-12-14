using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFA.DAS.Data.Pipeline.Helpers;
using SFA.DAS.Messaging;

namespace SFA.DAS.Data.Pipeline.Tests
{
    public class TestMessage<T> : Message<T>
    {
        public TestMessage(T thing) : base(thing) {}

        public override Task CompleteAsync()
        {
            throw new System.NotImplementedException();
        }

        public override Task AbortAsync()
        {
            throw new System.NotImplementedException();
        }
    }

    [TestClass]
    public class MessageHelperTests
    {
        public class Test
        {
            public string Value { get; set; }
        }

        public async Task<Message<Test>> GetMessage()
        {
            return await Task.Run(() => new TestMessage<Test>(new Test {Value = "bob"}));
        }

        public async Task<Message<Test>> GetEmptyMessage()
        {
            return await Task.Run(() => new TestMessage<Test>(null));
        }

        [TestMethod]
        public void FetchMessage()
        {
            var result = MessageQueue
                .WaitFor(GetMessage)
                .Step(m => Result.Win(new Test {Value = "hello " + m.Value}, "recived bob"));

            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual("hello bob", result.Content.Value);
        }

        [TestMethod]
        public void EmptyMessage()
        {
            var result = MessageQueue
                .WaitFor(GetEmptyMessage)
                .Step(m => Result.Win(new Test { Value = "hello " + m.Value }, "recived bob"));

            Assert.IsFalse(result.IsSuccess());
        }
    }
}