IF (NOT EXISTS(SELECT * FROM [Data_Load].[DAS_LoadedEvents] WHERE [EventFeed] = 'AccountEvents'))
BEGIN 
	INSERT INTO [Data_Load].[DAS_LoadedEvents] ([EventFeed], [LastProcessedEventId]) 
	VALUES('AccountEvents', 0) 
END 


IF (SELECT COUNT(*) FROM Data_Load.DAS_Employer_Accounts) = 0
BEGIN
	DELETE FROM Data_Load.DAS_FailedEvents
	UPDATE Data_Load.DAS_LoadedEvents SET LastProcessedEventId = 0 WHERE EventFeed = 'AccountEvents'
END

IF DATABASE_PRINCIPAL_ID('ViewSpecificReadOnly') IS NOT NULL
BEGIN
	GRANT SELECT ON [Data_Pub].[DAS_Employer_Accounts] TO ViewSpecificReadOnly
	GRANT SELECT ON [Data_Pub].[DAS_Employer_LegalEntities] TO ViewSpecificReadOnly
	GRANT SELECT ON [Data_Pub].[DAS_Employer_PayeSchemes] TO ViewSpecificReadOnly
	GRANT SELECT ON [Data_Pub].[DAS_Employer_Registrations] TO ViewSpecificReadOnly
END