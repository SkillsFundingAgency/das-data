CREATE PROCEDURE [Data_Load].[SaveProvider]
	@ukprn bigint,
	@uri nvarchar(max),
	@providerTypeId int,
	@providerTypeDescription nvarchar(max),
	@parentCompanyGuarantee bit,
	@newOrganisationWithoutFinancialTrackRecord bit,
	@startDate datetime = NULL
AS

	SET NOCOUNT ON;
	SET ANSI_NULLS OFF

	UPDATE [Data_Load].[Provider] SET IsLatest = 0 WHERE [Ukprn] = @ukprn

	INSERT INTO [Data_Load].[Provider]
	(
		[Ukprn],
		[Uri],
		[ProviderTypeId],
		[ProviderTypeDescription],
		[ParentCompanyGuarantee],
		[NewOrganisationWithoutFinancialTrackRecord],
		[StartDate],
		[IsLatest]
	)
	VALUES
	(
		@ukprn,
		@uri,
		@providerTypeId,
		@providerTypeDescription,
		@parentCompanyGuarantee,
		@newOrganisationWithoutFinancialTrackRecord,
		@startDate,
		1
	)