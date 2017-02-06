IF (NOT EXISTS(SELECT * FROM [Data_Load].[DAS_LoadedEvents] WHERE [EventFeed] = 'AccountEvents'))
BEGIN 
	INSERT INTO [Data_Load].[DAS_LoadedEvents] ([EventFeed], [LastProcessedEventId]) 
	VALUES('AccountEvents', 0) 
END 


IF (SELECT COUNT(*) FROM Data_Load.DAS_Employer_Accounts) = 0
BEGIN
	DELETE FROM Data_Load.DAS_FailedEvents
	UPDATE Data_Load.DAS_LoadedEvents SET LastProcessedEventId = 0 WHERE EventFeed = 'Accounts'
END