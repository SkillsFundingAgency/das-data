CREATE PROCEDURE [Data_Load].[SaveProviderAddress]
	@UkPrn BIGINT,
	@ContactType NVARCHAR(50),
	@Street NVARCHAR(255) = NULL,
	@Town NVARCHAR(255) = NULL,
	@Postcode NVARCHAR(25) = NULL
AS
	INSERT INTO [Data_Load].[DAS_ProviderAddress]
	(
		[UkPrn],
		[ContactType],
		[Street],
		[Town],
		[Postcode]
	)
	VALUES
	(
		@UkPrn,
		@ContactType,
		@Street,
		@Town,
		@Postcode
	)