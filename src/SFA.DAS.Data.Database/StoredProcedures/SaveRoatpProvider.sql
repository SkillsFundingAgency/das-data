CREATE PROCEDURE [RoATP].[SaveRoatpProvider]
	@ukprn bigint,
	@uri nvarchar(max),
	@providerType int,
	@parentCompanyGuarantee bit,
	@newOrganisationWithoutFinancialTrackRecord bit,
	@startDate datetime = NULL
AS

	SET NOCOUNT ON;
	SET ANSI_NULLS OFF

	UPDATE [RoATP].[Provider] SET IsLatest = 0 WHERE [Ukprn] = @ukprn

	INSERT INTO [RoATP].[Provider]
	(
		[Ukprn],
		[Uri],
		[ProviderType],
		[ParentCompanyGuarantee],
		[NewOrganisationWithoutFinancialTrackRecord],
		[StartDate],
		[IsLatest]
	)
	VALUES
	(
		@ukprn,
		@uri,
		@providerType,
		@parentCompanyGuarantee,
		@newOrganisationWithoutFinancialTrackRecord,
		@startDate,
		1
	)