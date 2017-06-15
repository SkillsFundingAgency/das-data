CREATE PROCEDURE [Data_Load].[SaveLegalEntity]
	@dasAccountId NVARCHAR(100),
	@dasLegalEntityId BIGINT,
	@name NVARCHAR(100),
	@address NVARCHAR(256) = NULL,
	@source NVARCHAR(50),
	@inceptionDate DATETIME = NULL,
	@code NVARCHAR(50) = NULL,
	@status NVARCHAR(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

    IF NOT EXISTS(
		SELECT * FROM [Data_Load].[DAS_Employer_LegalEntities]
		WHERE 
			[DasAccountId] = @dasAccountId AND
			[DasLegalEntityId] = @dasLegalEntityId AND
			[Name] = @name AND
			[Address] = @address AND
			[Source] = @source AND
			[InceptionDate] = @inceptionDate AND
			[Code] = @code AND
			[Status] = @status
	)
	BEGIN
		UPDATE [Data_Load].[DAS_Employer_LegalEntities] SET IsLatest = 0 WHERE [DasAccountId] = @dasAccountId AND [DasLegalEntityId] = @dasLegalEntityId

		INSERT INTO [Data_Load].[DAS_Employer_LegalEntities] ([DasAccountId],[DasLegalEntityId],[Name],[Address],[Source],[InceptionDate],[Code],[Status], [IsLatest])
			VALUES (@dasAccountId, @dasLegalEntityId, @name, @address, @source, @inceptionDate, @code, @status, 1)
	END
END
GO
