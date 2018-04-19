CREATE TABLE [Data_Load].[DAS_Employer_Account_Transfers]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[SenderAccountId] BIGINT NOT NULL, 
    [ReceiverAccountId] BIGINT NOT NULL, 
    [RequiredPaymentId] UNIQUEIDENTIFIER NOT NULL, 
	[CommitmentId] BIGINT NOT NULL,
	[Amount]	DECIMAL(18, 5) NULL,
	[Type] NVARCHAR(50) NOT NULL,
	[TransferDate] DATE NOT NULL,
    [ColectionPeriodName] NCHAR(10) NOT NULL, 
    [UpdateDateTime] DATETIME NOT NULL DEFAULT (GETDATE()) 
)
GO

CREATE INDEX [IX_DAS_Employer_Account_Transfers_SenderAccountId] ON [Data_Load].[DAS_Employer_Account_Transfers] ([SenderAccountId])
GO

CREATE INDEX [IX_DAS_Employer_Account_Transfers_ReceiverAccountId] ON [Data_Load].[DAS_Employer_Account_Transfers] ([ReceiverAccountId])
GO
