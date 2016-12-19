using NUnit.Framework;
using SFA.DAS.Data.Pipeline.Helpers;
using Simple.Data;
using Assert = NUnit.Framework.Assert;

namespace SFA.DAS.Data.Pipeline.UnitTests
{
    [TestFixture]
    public class DatabaseExtensionTests
    {
        public class Message
        {
            public string Value { get; set; }
        }

        [Test]
        public void StoreItem()
        {
            Database.UseMockAdapter(new InMemoryAdapter());
            var db = Database.Open();
            var conn = new DbWrapper {Wrapper = db};

            var message = new Message {Value = "bob"};

            var result = message.Return()
                .Step(m => Result.Win(new Message {Value = "hello " + m.Value}, "said hello"))
                .Store(conn, "messages");

            var record = db.messages.FindByValue("hello bob");
            Assert.AreEqual("hello bob",record.Value);

            Assert.IsTrue(result.IsSuccess());
        }
    }
}