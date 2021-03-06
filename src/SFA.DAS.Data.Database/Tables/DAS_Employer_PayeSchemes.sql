﻿CREATE TABLE [Data_Load].[DAS_Employer_PayeSchemes]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [DasAccountId] NVARCHAR(100) NOT NULL, 
    [Ref] VARCHAR(20) NOT NULL, 
    [Name] NVARCHAR(100) NULL, 
    [AddedDate] DATETIME NOT NULL, 
    [RemovedDate] DATETIME NULL, 
    [UpdateDateTime] DATETIME NOT NULL DEFAULT (GETDATE()), 
    [IsLatest] BIT NOT NULL DEFAULT 0
)
GO
CREATE INDEX [IX_PayeScheme_AccountId] ON [Data_Load].[DAS_Employer_PayeSchemes] ([DasAccountId], [IsLatest])
GO
CREATE INDEX [IX_PayeScheme_Account_Ref] ON [Data_Load].[DAS_Employer_PayeSchemes] ([DasAccountId], [Ref], [IsLatest])
GO
CREATE INDEX [IX_PayeScheme_IsLatest] ON [Data_Load].[DAS_Employer_PayeSchemes] ([IsLatest])