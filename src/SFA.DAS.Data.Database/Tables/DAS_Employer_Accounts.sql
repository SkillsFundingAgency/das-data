CREATE TABLE [Data_Load].[DAS_Employer_Accounts]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [DasAccountId] NVARCHAR(100) NOT NULL, 
    [AccountName] NVARCHAR(100) NOT NULL, 
    [DateRegistered] DATETIME NOT NULL, 
    [OwnerEmail] NVARCHAR(255) NOT NULL, 
    [UpdateDateTime] DATETIME NOT NULL DEFAULT (GETDATE()), 
    [AccountId] BIGINT NOT NULL DEFAULT 0
)
GO
CREATE INDEX [IX_Account_AccountId] ON [Data_Load].[DAS_Employer_Accounts] ([AccountId])
GO
CREATE INDEX [IX_Account_DasAccountId] ON [Data_Load].[DAS_Employer_Accounts] ([DasAccountId], [UpdateDateTime])