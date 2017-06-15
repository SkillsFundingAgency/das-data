CREATE PROCEDURE [Data_Load].[SavePayeScheme]
	@dasAccountId NVARCHAR(100),
	@ref NVARCHAR(20),
	@name NVARCHAR(100) = NULL,
	@addedDate DATETIME,
	@removedDate DATETIME = NULL
AS
BEGIN
	SET NOCOUNT ON;
	SET ANSI_NULLS OFF

    IF NOT EXISTS(
		SELECT * FROM [Data_Load].[DAS_Employer_PayeSchemes]
		WHERE 
			[DasAccountId] = @dasAccountId AND
			[Ref] = @ref AND
			[Name] = @name AND
			[AddedDate] = @addedDate AND
			[RemovedDate] = @removedDate
	)
	BEGIN

		UPDATE [Data_Load].[DAS_Employer_PayeSchemes] SET [IsLatest] = 0 WHERE [DasAccountId] = @dasAccountId AND [Ref] = @ref

		INSERT INTO [Data_Load].[DAS_Employer_PayeSchemes] ([DasAccountId],[Ref],[Name],[AddedDate], [RemovedDate], [IsLatest])
			VALUES (@dasAccountId, @ref, @name, @addedDate, @removedDate, 1)
	END
END
GO
