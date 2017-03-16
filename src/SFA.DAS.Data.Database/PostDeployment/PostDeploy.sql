IF NOT EXISTS(SELECT TOP 1 * FROM Data_Load.DAS_LoadedEvents WHERE EventFeed = 'AccountEventView')
BEGIN
	UPDATE Data_Load.DAS_LoadedEvents SET EventFeed = 'AccountEventView' WHERE EventFeed = 'AccountEvents'
END

IF DATABASE_PRINCIPAL_ID('ViewSpecificReadOnly') IS NOT NULL
BEGIN
	GRANT SELECT ON [Data_Pub].[DAS_Employer_Accounts] TO ViewSpecificReadOnly
	GRANT SELECT ON [Data_Pub].[DAS_Employer_LegalEntities] TO ViewSpecificReadOnly
	GRANT SELECT ON [Data_Pub].[DAS_Employer_PayeSchemes] TO ViewSpecificReadOnly
	GRANT SELECT ON [Data_Pub].[DAS_Commitments] TO ViewSpecificReadOnly
END