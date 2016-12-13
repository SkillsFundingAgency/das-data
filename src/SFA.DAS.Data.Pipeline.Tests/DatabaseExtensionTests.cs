using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFA.DAS.Data.Pipeline.Helpers;
using Simple.Data;

namespace SFA.DAS.Data.Pipeline.Tests
{
    [TestClass]
    public class DatabaseExtensionTests
    {
        public class Message
        {
            public string Value { get; set; }
        }

        [TestMethod]
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