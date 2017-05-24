CREATE TABLE [Data_Load].[DAS_Roatp]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[UkPrn] BIGINT NOT NULL,
	[ProviderType] NVARCHAR(50) NOT NULL,
	[ParentCompanyGuarantee] BIT NOT NULL,
	[NewOrganisationWithoutFinancialTrackRecord] BIT NOT NULL,
	[StartDate] DATETIME NOT NULL,
	[UpdateDateTime] DATETIME NOT NULL DEFAULT (GETDATE())
)
