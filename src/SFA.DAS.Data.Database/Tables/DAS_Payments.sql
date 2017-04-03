CREATE TABLE [Data_Load].[DAS_Payments]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[PaymentId]	BIGINT NOT NULL,
	[UkPrn]	BIGINT NOT NULL,
	[Uln]	BIGINT NOT NULL,
	[EmployerAccountId]	NVARCHAR(100)  NOT NULL,
	[ApprenticeshipId]	BIGINT NULL,
	[DeliveryMonth]	INT NOT NULL,
	[DeliveryYear]	INT NOT NULL,
	[CollectionMonth]	INT NOT NULL,
	[CollectionYear]	INT NOT NULL,
	[EvidenceSubmittedOn]	DATETIME NOT NULL,
	[EmployerAccountVersion]	NVARCHAR(50) NOT NULL,
	[ApprenticeshipVersion]	NVARCHAR(50) NOT NULL,
	[FundingSource]	NVARCHAR(25) NOT NULL,
	[TransactionType]	NVARCHAR(25) NOT NULL,
	[Amount]	DECIMAL NOT NULL,
	[StandardCode]	BIGINT NULL,
	[FrameworkCode]	INT NULL,
	[ProgrammeType]	INT NULL,
	[PathwayCode]	INT NULL,
	[ContractType]	NVARCHAR(50) NOT NULL
)
