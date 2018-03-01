﻿CREATE PROCEDURE [Data_Load].[SaveRelationship]
	@employerAccountId BIGINT,
	@legalEntityId NVARCHAR(100),
	@legalEntityName NVARCHAR(100) = NULL,
	@legalEntityAddress NVARCHAR(256) = NULL,
	@legalEntityOrganisationTypeId TINYINT = NULL,
	@legalEntityOrganisationTypeDescription NVARCHAR(MAX) = NULL,
	@providerId BIGINT,
	@providerName NVARCHAR(100) = NULL,
	@verified BIT
AS
	SET NOCOUNT ON;
	SET ANSI_NULLS OFF

	DECLARE @id BIGINT = 0

	SELECT	@id = Id
	FROM	[Data_Load].[DAS_Relationship] WITH (NOLOCK)
	WHERE	EmployerAccountId = @employerAccountId AND
			ProviderId = @providerId AND
			LegalEntityId = @legalEntityId
			AND IsLatest = 1

	IF @id > 0
	BEGIN
		UPDATE  [Data_Load].[DAS_Relationship]
		SET		IsLatest = 0
		WHERE	Id = @id
		
		INSERT INTO [Data_Load].[DAS_Relationship]
		SELECT	ProviderId, 
				ProviderName,
				EmployerAccountId,
				LegalEntityId,
				LegalEntityName,
				LegalEntityAddress,
				LegalEntityOrganisationTypeId,
				LegalEntityOrganisationTypeDescription,
				@verified,
				1
		FROM [Data_Load].[DAS_Relationship] WITH (NOLOCK)
		WHERE Id =@id
	END
	ELSE
	BEGIN
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
				(@providerId,
				@providerName,
				@employerAccountId, 
				@legalEntityId, 
				@legalEntityName, 
				@legalEntityAddress,
				@legalEntityOrganisationTypeId, 
				@legalEntityOrganisationTypeDescription,
				@verified,
				1)
	END