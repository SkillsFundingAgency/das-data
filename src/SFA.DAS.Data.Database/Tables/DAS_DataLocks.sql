CREATE TABLE [Data_Lock].[DAS_DataLocks]
(
	[Id] [bigint] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Collection] NVARCHAR(10) NOT NULL,
	[Ukprn] BIGINT NOT NULL,
	[LearnRefNumber] NVARCHAR(12) NOT NULL,
	[ULN] [BIGINT] NOT NULL,
	[AimSeqNumber] BIGINT NOT NULL,
	[RuleId] [NVARCHAR](10) NOT NULL,
	[CollectionPeriodName] NVARCHAR(10) NOT NULL,
	[CollectionPeriodMonth] INT NOT NULL,
	[CollectionPeriodYear] INT NOT NULL,
	[LastSubmission] DATETIME NULL,
	[TNP] BIGINT NOT NULL
)
