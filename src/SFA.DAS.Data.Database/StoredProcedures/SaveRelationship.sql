CREATE PROCEDURE [Data_Load].[SaveRelationship]
	@employerAccountId BIGINT,
	@legalEntityId NVARCHAR(100),
	@legalEntityName NVARCHAR(100),
	@legalEntityAddress NVARCHAR(256),
	@legalEntityOrganisationTypeId TINYINT,
	@legalEntityOrganisationTypeDescription NVARCHAR(MAX),
	@providerId BIGINT,
	@providerName NVARCHAR(100),
	@verified BIT
AS
	SET NOCOUNT ON;
	SET ANSI_NULLS OFF

	IF NOT EXISTS(
		SELECT	TOP 1 1 
		FROM	[Data_Load].DAS_Relationship 
		WHERE	employerAccountId = @employerAccountId AND
				providerId = @providerId)
			BEGIN
				UPDATE [Data_Load].DAS_Relationship 
				SET IsLatest = 0
				WHERE	employerAccountId = @employerAccountId AND
				providerId = @providerId
			END

	INSERT INTO [Data_Load].[DAS_Relationship]
				   ([ProviderId]
				   ,[ProviderName]
				   ,[EmployerAccountId]
				   ,[LegalEntityId]
				   ,[LegalEntityName]
				   ,[LegalEntityAddress]
				   ,[LegalEntityOrganisationTypeId]
				   ,[LegalEntityOrganisationTypeDescription]
				   ,[Verified]
				   ,[IsLatest])
			 VALUES
				   (@ProviderId,
				   @ProviderName,
				   @EmployerAccountId, 
				   @LegalEntityId, 
				   @LegalEntityName, 
				   @LegalEntityAddress,
				   @LegalEntityOrganisationTypeId, 
				   @LegalEntityOrganisationTypeDescription,
				   @Verified,
				   1)
