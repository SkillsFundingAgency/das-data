CREATE PROCEDURE [Data_Load].[SaveRoatp]
	@UkPrn BIGINT,
	@ProviderType NVARCHAR(50),
	@ParentCompanyGuarantee BIT,
	@NewOrganisationWithoutFinancialTrackRecord BIT,
	@StartDate DATETIME
AS

	INSERT INTO [Data_Load].[DAS_Roatp]
	(
		[UkPrn],
		[ProviderType],
		[ParentCompanyGuarantee],
		[NewOrganisationWithoutFinancialTrackRecord],
		[StartDate]
	)
	VALUES
	(
		@UkPrn,
		@ProviderType,
		@ParentCompanyGuarantee,
		@NewOrganisationWithoutFinancialTrackRecord,
		@StartDate
	)
