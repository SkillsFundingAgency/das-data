using System;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Tests.Builders
{
    public class DataLockEventBuilder
    {
        private long _id = 1234;
        private DateTime _processDateTime = DateTime.Now;
        private int _ukPrn = 12345;
        private long _uln = 123;

        private string _learnRefNumber = "Lrn-001";
        private long _aimSeqNumber = 1;
        private string _priceEpisodeIdentifier = "25-27-01/05/2017";
        private long _apprenticeshipId = 1;
        private long _employerAccountId = 999;
        private EventSource _eventSource = EventSource.Submission;
        private bool _hasErrors = true;
        private string _ilrFileName = "ILR-123456";
        private DateTime? _ilrStartDate = new DateTime(2017, 05, 01);
        private int? _ilrStandardCode = 27;
        private int? _ilrProgrammeType = 20;
        private int? _ilrFrameworkCode = 550;
        private int? _ilrPathwayCode = 6;
        private Decimal? _ilrTrainingPrice = 12000;
        private Decimal? _ilrEndpointAssessorPrice = 3000;
        private DateTime? _ilrPriceEffectiveFromDate = new DateTime(2017, 04, 01);
        private DateTime? _ilrPriceEffectiveToDate = new DateTime(2018, 03, 31);

        private string _errorCode = "DLOCK_07";
        private string _systemDescription = "Mismatch on price";

        private string _apprenticeshipVersion = "99";
        private string _periodId = "1617-R10";
        private int _periodMonth = 5;
        private int _periodYear = 2017;

        private bool _isPayable = false;
        private TransactionType _transactionType = TransactionType.Learning;

        private DateTime _startDate = new DateTime(2017, 05, 01);
        private long? _standardCode = 27;
        private int? _programmeType = 20;
        private int? _frameworkCode = 550;
        private int? _pathwayCode = 6;

        private Decimal _negotiatedPrice = 17500;
        private DateTime _effectiveDate = new DateTime(2017, 05, 01);
        private DataLockEventError[] _errors = null;

        public DataLockEventBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public DataLockEventBuilder WithNoErrors()
        {
            _hasErrors = false;
            _errors = new DataLockEventError[0];

            return this;
        }
        
        public DataLockEventBuilder WithMultipleErrors(int numberOfErrors)
        {
            _hasErrors = true;
            _errors = new DataLockEventError[numberOfErrors];
            for (int i = 0; i < numberOfErrors; i++)
            {
                _errors[i] = new DataLockEventError
                {
                    ErrorCode = $"DLOCK-{i + 1:##}",
                    SystemDescription = $"Description for error {i + 1:##}"
                };
            }

            return this;
        }

        public DataLockEvent Build()
        {
            return new DataLockEvent
            {
                Id = _id,
                ProcessDateTime = _processDateTime,
                IlrFileName = _ilrFileName,
                Ukprn = _ukPrn,
                Uln = _uln,
                LearnRefNumber = _learnRefNumber,
                AimSeqNumber = _aimSeqNumber,
                PriceEpisodeIdentifier = _priceEpisodeIdentifier,
                ApprenticeshipId = _apprenticeshipId,
                EmployerAccountId = _employerAccountId,
                EventSource = _eventSource,
                HasErrors = _hasErrors,
                IlrStartDate = _ilrStartDate,
                IlrStandardCode = _ilrStandardCode,
                IlrProgrammeType = _ilrProgrammeType,
                IlrFrameworkCode = _ilrFrameworkCode,
                IlrPathwayCode   = _ilrPathwayCode,
                IlrTrainingPrice = _ilrTrainingPrice,
                IlrEndpointAssessorPrice = _ilrEndpointAssessorPrice,
                IlrPriceEffectiveFromDate = _ilrPriceEffectiveFromDate,
                IlrPriceEffectiveToDate = _ilrPriceEffectiveToDate,
            Errors = _errors ?? new[]
                {
                    new DataLockEventError
                    {
                        ErrorCode = _errorCode,
                        SystemDescription = _systemDescription
                    }
                },
                Periods = new[]
                {
                    new DataLockEventPeriod
                    {
                        ApprenticeshipVersion = _apprenticeshipVersion,
                        Period = new NamedCalendarPeriod
                        {
                            Id = _periodId,
                            Month = _periodMonth ,
                            Year = _periodYear
                        },
                        IsPayable = _isPayable,
                        TransactionType= _transactionType
                   },
                },

                Apprenticeships = new[]
                {
                    new DataLockEventApprenticeship
                    {
                        Version = _apprenticeshipVersion,
                        StartDate   = _startDate,
                        StandardCode = _standardCode ,
                        ProgrammeType =_programmeType,
                        FrameworkCode =_frameworkCode,
                        PathwayCode =  _pathwayCode ,
                        NegotiatedPrice = _negotiatedPrice,
                        EffectiveDate = _effectiveDate
                    }
                }
            };
        }
    }
}
