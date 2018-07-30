CREATE VIEW [Data_Pub].[DAS_Commitments_Relationships]
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
	FROM [Data_Load].[DAS_Commitments_Relationships] 
	WHERE IsLatest = 1