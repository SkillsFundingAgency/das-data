CREATE TABLE [Data_Load].[DAS_Payments]
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
	[Amount]	DECIMAL(18, 5) NULL,
	[StandardCode]	BIGINT NULL,
	[FrameworkCode]	INT NULL,
	[ProgrammeType]	INT NULL,
	[PathwayCode]	INT NULL,
	[ContractType]	NVARCHAR(50) NULL, 
    [UpdateDateTime] DATETIME NOT NULL DEFAULT (GETDATE()), 
    [FundingAccountId] BIGINT NULL
)
GO
CREATE INDEX [IX_Payment_FundingSource] ON [Data_Load].[DAS_Payments] ([FundingSource])
GO
CREATE INDEX [IX_Payment_ApprenticeshipDeliveryMonth] ON [Data_Load].[DAS_Payments] ([EmployerAccountId], [ApprenticeshipId], [DeliveryMonth], [DeliveryYear])
GO
CREATE INDEX [IX_Payment_Delivery_FundingSource] ON [Data_Load].[DAS_Payments] ([DeliveryYear], [DeliveryMonth], [FundingSource])
GO
CREATE INDEX [IX_Payment_TransactionType_Delivery] ON [Data_Load].[DAS_Payments] ([TransactionType], [DeliveryYear], [DeliveryMonth])
GO
CREATE INDEX [IX_Payment_DeliveryMonth_DeliveryYear] ON [Data_Load].[DAS_Payments] ([DeliveryMonth], [DeliveryYear]) INCLUDE ([Amount], [ApprenticeshipId], [EmployerAccountId], [FundingSource], [UpdateDateTime])
GO
CREATE INDEX [IX_Payment_PaymentId] ON [Data_Load].[DAS_Payments] ([PaymentId])