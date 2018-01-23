CREATE TABLE [RoATP].[Provider]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[Ukprn] BIGINT NOT NULL,
	[Uri] NVARCHAR(MAX) NULL,
	[ProviderType] INT NOT NULL FOREIGN KEY REFERENCES [RoATP].ProviderType(Id),
	[ParentCompanyGuarantee] BIT NOT NULL,
	[NewOrganisationWithoutFinancialTrackRecord] BIT NOT NULL,
	[StartDate] DATETIME NULL,
	[IsLatest] BIT NOT NULL DEFAULT 0
)
