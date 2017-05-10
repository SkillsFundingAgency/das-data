﻿CREATE TABLE [Data_Load].[DAS_Payments]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[PaymentId]	NVARCHAR(100) NULL,
	[UkPrn]	BIGINT NULL,
	[Uln]	BIGINT NULL,
	[EmployerAccountId]	NVARCHAR(100) NULL,
	[ApprenticeshipId]	BIGINT NULL,
	[DeliveryMonth]	INT NULL,
	[DeliveryYear]	INT NULL,
	[CollectionMonth]	INT NULL,
	[CollectionYear]	INT NULL,
	[EvidenceSubmittedOn]	DATETIME NULL,
	[EmployerAccountVersion]	NVARCHAR(50) NULL,
	[ApprenticeshipVersion]	NVARCHAR(50) NULL,
	[FundingSource]	NVARCHAR(25) NULL,
	[TransactionType]	NVARCHAR(50) NULL,
	[Amount]	DECIMAL NULL,
	[StandardCode]	BIGINT NULL,
	[FrameworkCode]	INT NULL,
	[ProgrammeType]	INT NULL,
	[PathwayCode]	INT NULL,
	[ContractType]	NVARCHAR(50) NULL, 
    [UpdateDateTime] DATETIME NOT NULL DEFAULT (GETDATE())
)
