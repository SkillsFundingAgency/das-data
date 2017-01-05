CREATE TABLE [data].[Registration]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY,
	[DasAccountName] NVARCHAR(100) NOT NULL,
	[DasRegistered] DATETIME NOT NULL,
	[LegalEntityRegisteredAddress] NVARCHAR(256) NULL,
	[LegalEntitySource] NVARCHAR(50) NOT NULL,
	[LegalEntityStatus] NVARCHAR(50),
	[LegalEntityName] NVARCHAR(100) NOT NULL,
	[LegalEntityCreatedDate] DATETIME,
	[OwnerEmail] NVARCHAR(255) NOT NULL,
	[DasAccountId] NVARCHAR(100), 
    [LegalEntityId] INT NULL, 
    [CompaniesHouseNumber] INT NULL,
	[UpdateDateTime] DATETIME NOT NULL DEFAULT(GETDATE())
)
