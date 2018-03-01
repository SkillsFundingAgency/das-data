CREATE VIEW [Data_Load].[DAS_Relationships]
	AS SELECT 
		[ProviderId],
		[ProviderName],
		[EmployerAccountId],
		[LegalEntityId],
		[LegalEntityName],
		[LegalEntityAddress],
		[LegalEntityOrganisationTypeId],
		[LegalEntityOrganisationTypeDescription],
		[Verified]
	FROM [Data_Load].[DAS_Relationship] 
	WHERE IsLatest = 1
