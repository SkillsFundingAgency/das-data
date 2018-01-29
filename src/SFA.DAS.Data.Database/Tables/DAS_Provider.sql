CREATE TABLE [Data_Load].[Provider]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[Ukprn] BIGINT NOT NULL,
	[Uri] NVARCHAR(MAX) NULL,
	[ProviderTypeId] INT NULL,
	[ProviderTypeDescription] NVARCHAR(MAX) NULL,
	[ParentCompanyGuarantee] BIT NOT NULL,
	[NewOrganisationWithoutFinancialTrackRecord] BIT NOT NULL,
	[StartDate] DATETIME NULL,
	[IsLatest] BIT NOT NULL DEFAULT 0
)
GO
CREATE INDEX [IX_Provider_Ukprn_IsLatest] ON [Data_Load].[Provider] ([Ukprn], [IsLatest])
