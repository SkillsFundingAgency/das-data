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
		INSERT INTO [Data_Load].[DAS_Employer_PayeSchemes] ([DasAccountId],[Ref],[Name],[AddedDate], [RemovedDate])
			VALUES (@dasAccountId, @ref, @name, @addedDate, @removedDate)
	END
END
GO
