CREATE TABLE [Data_Load].[DAS_Employer_LegalEntity]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [DasAccountId] NVARCHAR(100) NOT NULL, 
    [DasLegalEntityId] NCHAR(10) NOT NULL, 
    [Name] NVARCHAR(100) NOT NULL, 
    [Address] NVARCHAR(256) NULL, 
    [Source] NVARCHAR(50) NOT NULL, 
    [InceptionDate] DATETIME NULL, 
    [Code] NVARCHAR(50) NULL, 
    [Status] NVARCHAR(50) NULL, 
    [UpdateDate] DATETIME NOT NULL DEFAULT (GETDATE())
)
