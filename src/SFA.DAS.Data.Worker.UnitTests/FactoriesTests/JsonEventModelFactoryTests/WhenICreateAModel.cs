using System;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.Data.Worker.Factories;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.UnitTests.FactoriesTests.JsonEventModelFactoryTests
{
    public class WhenICreateAModel
    {
        private JsonEventModelFactory _factory;
        private Mock<ILog> _logger;

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILog>();
            _factory = new JsonEventModelFactory(_logger.Object);
        }

        [Test]
        public void ThenIShouldGetAFullyPopulatedModel()
        {
            //Arange
            const string data = "test data";
            const string modelString = "{\"Data\":\"" + data + "\"}";

            //Act
            var model = _factory.Create<TestEvent>(modelString);

            //Assert
            Assert.IsNotNull(model);
            Assert.AreEqual(data, model.Data);
        }

        [Test]
        public void ThenIShouldGetNullIfTheModelCannotBeCreated()
        {
            //Act
            var model = _factory.Create<TestEvent>("will not deserialise");

            //Assert
            Assert.IsNull(model);
        }

        [Test]
        public void ThenIShouldLogErrorIfModelCannotBeCreated()
        {
            //Arange
            _logger.Setup(x => x.Error(It.IsAny<Exception>(), It.IsAny<string>()));

            //Act
            var model = _factory.Create<TestEvent>("Not a model");

            //Assert


            _logger.Verify(x => x.Error(It.IsAny<Exception>(), "Could not create model of type"));
        }

        internal class TestEvent
        {
            public string Data { get; set; }
        }
    }
}
