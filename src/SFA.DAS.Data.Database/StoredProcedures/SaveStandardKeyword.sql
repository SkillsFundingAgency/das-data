CREATE PROCEDURE [Data_Load].[SaveStandardKeyword]
	@StandardId NVARCHAR(255),
	@Keyword NVARCHAR(255)
AS
	IF NOT EXISTS(
		SELECT * FROM [Data_Load].[DAS_Standard_Keywords]
		WHERE 
			[StandardId] = @StandardId AND
			[Keyword] = @Keyword
	)
	BEGIN

		INSERT INTO [Data_Load].[DAS_Standard_Keywords]
		(
			[StandardId],
			[Keyword]
		)
		VALUES
		(
			@StandardId,
			@Keyword
		)

	END
