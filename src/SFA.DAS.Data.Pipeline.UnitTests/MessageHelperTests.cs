using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Data.Pipeline.Helpers;
using SFA.DAS.Messaging;

namespace SFA.DAS.Data.Pipeline.UnitTests
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

    [TestFixture]
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

        [Test]
        public void FetchMessage()
        {
            var result = MessageQueue
                .WaitFor(GetMessage)
                .Step(m => Result.Win(new Test {Value = "hello " + m.Value}, "recived bob"));

            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual("hello bob", result.Content.Value);
        }

        [Test]
        public void EmptyMessage()
        {
            var result = MessageQueue
                .WaitFor(GetEmptyMessage)
                .Step(m => Result.Win(new Test { Value = "hello " + m.Value }, "recived bob"));

            Assert.IsFalse(result.IsSuccess());
        }
    }
}