﻿CREATE TABLE [Data_Load].[DAS_Employer_Accounts]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [DasAccountId] NVARCHAR(100) NOT NULL, 
    [AccountName] NVARCHAR(100) NOT NULL, 
    [DateRegistered] DATETIME NOT NULL, 
    [OwnerEmail] NVARCHAR(255) NOT NULL, 
    [UpdateDateTime] DATETIME NOT NULL DEFAULT (GETDATE()), 
    [AccountId] BIGINT NOT NULL DEFAULT 0, 
    [IsLatest] BIT NOT NULL DEFAULT 0
)
GO
CREATE INDEX [IX_Account_AccountId_IsLatest] ON [Data_Load].[DAS_Employer_Accounts] ([AccountId], [IsLatest])
GO
CREATE INDEX [IX_Account_DasAccountId_IsLatest] ON [Data_Load].[DAS_Employer_Accounts] ([DasAccountId], [IsLatest])
GO
CREATE INDEX [IX_Account_IsLatest_DateRegistered] ON [Data_Load].[DAS_Employer_Accounts] ([IsLatest], [DateRegistered])