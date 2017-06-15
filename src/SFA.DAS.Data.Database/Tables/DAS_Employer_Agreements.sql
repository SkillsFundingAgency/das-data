CREATE TABLE [Data_Load].[DAS_Employer_Agreements]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
	[DasAccountId] NVARCHAR(100) NOT NULL,
	[Status] NVARCHAR(50) NOT NULL,
	[SignedBy] NVARCHAR(100) NULL,
	[SignedDate] DATETIME NULL,
	[ExpiredDate] DATETIME NULL,
	[DasLegalEntityId] BIGINT NOT NULL,
	[DasAgreementId] NVARCHAR(100) NOT NULL, 
	[UpdateDateTime] DATETIME NOT NULL DEFAULT (GETDATE()),
	[IsLatest] BIT NOT NULL DEFAULT 0
)
GO
CREATE INDEX [IX_Agreement_DasAccountId_IsLatest] ON [Data_Load].[DAS_Employer_Agreements] ([DasAccountId], [IsLatest])
