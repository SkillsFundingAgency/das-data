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
	GRANT SELECT ON [Data_Pub].[DAS_CalendarMonth] TO ViewSpecificReadOnly
	GRANT SELECT ON [Data_Pub].[DAS_LevyDeclarations] TO ViewSpecificReadOnly
END

Exec Data_Load.UpdateCalendarMonth

IF (SELECT COUNT(*) FROM [Data_Load].[DAS_Employer_Accounts] WHERE IsLatest = 1) = 0
BEGIN
	UPDATE [Data_Load].[DAS_Employer_Accounts]
	SET IsLatest = 1
	WHERE Id IN (SELECT	MAX([Id]) FROM [Data_Load].[DAS_Employer_Accounts] GROUP BY [DasAccountId])
END

IF (SELECT COUNT(*) FROM [Data_Load].[DAS_Employer_LegalEntities] WHERE IsLatest = 1) = 0
BEGIN
	UPDATE [Data_Load].[DAS_Employer_LegalEntities]
	SET IsLatest = 1
	WHERE Id IN (SELECT	MAX([Id]) FROM [Data_Load].[DAS_Employer_LegalEntities] GROUP BY [DasAccountId], [DasLegalEntityId])
END