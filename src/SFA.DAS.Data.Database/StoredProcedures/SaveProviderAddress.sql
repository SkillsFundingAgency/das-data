CREATE PROCEDURE [Data_Load].[SaveProviderAddress]
	@UkPrn BIGINT,
	@ContactType NVARCHAR(50),
	@Primary NVARCHAR(255) = NULL,
	@Secondary NVARCHAR(255) = NULL,
	@Street NVARCHAR(255) = NULL,
	@Town NVARCHAR(255) = NULL,
	@Postcode NVARCHAR(25) = NULL
AS

	IF NOT EXISTS(
		SELECT * FROM [Data_Load].[DAS_ProviderAddress]
		WHERE 
			[UkPrn] = @UkPrn AND
			[ContactType] = @ContactType AND 
			[Primary] = @Primary AND 
			[Secondary] = @Secondary AND 
			[Street] = @Street AND
			[Town] = @Town AND
			[Postcode] = @Postcode
	)
	BEGIN

		INSERT INTO [Data_Load].[DAS_ProviderAddress]
		(
			[UkPrn],
			[ContactType],
			[Primary],
			[Secondary],
			[Street],
			[Town],
			[Postcode]
		)
		VALUES
		(
			@UkPrn,
			@ContactType,
			@Primary,
			@Secondary,
			@Street,
			@Town,
			@Postcode
		)
	
	END