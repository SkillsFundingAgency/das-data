CREATE PROCEDURE [Data_Load].[SaveStandardJobRole]
	@StandardId NVARCHAR(255),
	@JobRole NVARCHAR(255)
AS
	IF NOT EXISTS(
		SELECT * FROM [Data_Load].[DAS_Standard_JobRoles]
		WHERE 
			[StandardId] = @StandardId AND
			[JobRole] = @JobRole
	)
	BEGIN

		INSERT INTO [Data_Load].[DAS_Standard_JobRoles]
		(
			[StandardId],
			[JobRole]
		)
		VALUES
		(
			@StandardId,
			@JobRole
		)

	END
