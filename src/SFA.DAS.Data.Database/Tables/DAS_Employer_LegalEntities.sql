CREATE TABLE [Data_Load].[DAS_Employer_LegalEntities]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [DasAccountId] NVARCHAR(100) NOT NULL, 
    [DasLegalEntityId] BIGINT NOT NULL, 
    [Name] NVARCHAR(100) NOT NULL, 
    [Address] NVARCHAR(256) NULL, 
    [Source] NVARCHAR(50) NOT NULL, 
    [InceptionDate] DATETIME NULL, 
    [Code] NVARCHAR(50) NULL, 
    [Status] NVARCHAR(50) NULL, 
    [UpdateDateTime] DATETIME NOT NULL DEFAULT (GETDATE()), 
    [IsLatest] BIT NOT NULL DEFAULT 0
)
GO
CREATE INDEX [IX_LegalEntity_Details] ON [Data_Load].[DAS_Employer_LegalEntities] ([Source], [Code], [Name], [DasAccountId], [IsLatest])
GO
CREATE INDEX [IX_LegalEntity_AccountId] ON [Data_Load].[DAS_Employer_LegalEntities] ([DasAccountId], [IsLatest])
GO
CREATE INDEX [IX_LegalEntity_IsLatest_Source] ON [Data_Load].[DAS_Employer_LegalEntities] ([IsLatest], [Source])