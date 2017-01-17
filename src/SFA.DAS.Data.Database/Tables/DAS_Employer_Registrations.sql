CREATE TABLE [Data_Load].[DAS_Employer_Registrations]
(
	[Id] BIGINT IDENTITY NOT NULL PRIMARY KEY,
	[DasAccountId] NVARCHAR(100), 
	[DasAccountName] NVARCHAR(100) NOT NULL,
	[DateRegistered] DATETIME NOT NULL,
	[LegalEntityRegisteredAddress] NVARCHAR(256) NULL,
	[LegalEntitySource] NVARCHAR(50) NOT NULL,
	[LegalEntityStatus] NVARCHAR(50),
	[LegalEntityName] NVARCHAR(100) NOT NULL,
	[LegalEntityCreatedDate] DATETIME,
	[LegalEntityNumber] NVARCHAR(50) NULL,
	[OwnerEmail] NVARCHAR(255) NOT NULL,
    [LegalEntityId] INT NULL, 
	[UpdateDateTime] DATETIME NOT NULL DEFAULT(GETDATE()), 
    [PayeSchemeName] NVARCHAR(100) NULL
)
