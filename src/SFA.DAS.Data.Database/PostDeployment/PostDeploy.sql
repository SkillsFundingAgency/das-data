UPDATE [Data_Load].[DAS_LoadedEvents] SET EventFeed = 'AccountEventView' WHERE EventFeed = 'AccountEvents'

IF DATABASE_PRINCIPAL_ID('ViewSpecificReadOnly') IS NOT NULL
BEGIN
	GRANT SELECT ON [Data_Pub].[DAS_Employer_Accounts] TO ViewSpecificReadOnly
	GRANT SELECT ON [Data_Pub].[DAS_Employer_LegalEntities] TO ViewSpecificReadOnly
	GRANT SELECT ON [Data_Pub].[DAS_Employer_PayeSchemes] TO ViewSpecificReadOnly
	GRANT SELECT ON [Data_Pub].[DAS_Employer_Registrations] TO ViewSpecificReadOnly
END