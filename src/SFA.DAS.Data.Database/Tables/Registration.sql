CREATE TABLE [data].[Registration]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY,
	[DasAccountName] NVARCHAR(255),
	[DasRegistered] DATETIME,
	[LegalEntityRegisteredAddress] NVARCHAR(MAX),
	[LegalEntitySource] NVARCHAR(255),
	[LegalEntityStatus] NVARCHAR(255),
	[LegalEntityName] NVARCHAR(255),
	[LegalEntityCreatedDate] DATETIME,
	[OwnerEmail] NVARCHAR(255),
	[DasAccountId] NVARCHAR(255), 
    [LegalEntityId] INT NULL, 
    [CompaniesHouseNumber] INT NULL,
	[UpdateDateTime] DATETIME NOT NULL DEFAULT(GETDATE())
)
