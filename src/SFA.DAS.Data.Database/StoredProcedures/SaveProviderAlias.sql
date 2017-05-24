CREATE PROCEDURE [Data_Load].[SaveProviderAlias]
	@UkPrn BIGINT,
	@Alias NVARCHAR(255)
AS

	IF NOT EXISTS(
		SELECT * FROM [Data_Load].[DAS_ProviderAlias]
		WHERE 
			[UkPrn] = @UkPrn AND
			[Alias] = @Alias
	)
	BEGIN

		INSERT INTO [Data_Load].[DAS_ProviderAlias]
		(
			[UkPrn],
			[Alias]
		)
		VALUES
		(
			@UkPrn,
			@Alias
		)

	END