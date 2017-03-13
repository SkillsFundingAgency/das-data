using System;
using AutoMapper;
using NUnit.Framework;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Worker.Mapping;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Worker.UnitTests.Mapping.EventMappingTests
{
    internal class WhenIMapApprenticeshipEvents
    {
        private IMapper _mapper;
        private ApprenticeshipEventView _sourceEvent;

        [SetUp]
        public void Arrange()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<EventsMapping>();
            });

            _mapper = config.CreateMapper();
           
            _sourceEvent = new ApprenticeshipEventView
            {
                Event = "Test Event",
                AgreementStatus = AgreementStatus.BothAgreed,
                PaymentStatus = PaymentStatus.PendingApproval,
                ApprenticeshipId = 12,
                CreatedOn = DateTime.Now,
                EmployerAccountId = "1234",
                LearnerId = "3",
                PaymentOrder = 1,
                ProviderId = "3456",
                TrainingEndDate = DateTime.Now.AddMonths(6),
                TrainingStartDate = DateTime.Now.AddDays(1),
                TrainingId = "89797",
                TrainingTotalCost = 23456.67M,
                TrainingType = TrainingTypes.Standard
            };
        }

        [Test]
        public void ThenIShouldGetTheCorrectPaymentStatus()
        {
            //Act
            var target = _mapper.Map<CommitmentsApprenticeshipEvent>(_sourceEvent);

            //Assert
            var statusName = Enum.GetName(typeof(PaymentStatus), _sourceEvent.PaymentStatus);
            Assert.AreEqual(statusName, target.PaymentStatus);
        }

        [Test]
        public void ThenIShouldGetTheCorrectAgreementStatus()
        {
            //Act
            var target = _mapper.Map<CommitmentsApprenticeshipEvent>(_sourceEvent);

            //Assert
            var statusName = Enum.GetName(typeof(AgreementStatus), _sourceEvent.AgreementStatus);
            Assert.AreEqual(statusName, target.AgreementStatus);
        }

        [Test]
        public void ThenIShouldGetTheCorrectTrainingType()
        {
            //Act
            var target = _mapper.Map<CommitmentsApprenticeshipEvent>(_sourceEvent);

            //Assert
            var statusName = Enum.GetName(typeof(TrainingTypes), _sourceEvent.TrainingType);
            Assert.AreEqual(statusName, target.TrainingType);
        }
    }
}
