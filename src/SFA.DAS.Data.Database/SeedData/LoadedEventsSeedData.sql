IF (NOT EXISTS(SELECT * FROM [Data_Load].[DAS_LoadedEvents] WHERE [EventFeed] = 'AccountEvents'))
BEGIN 
	INSERT INTO [Data_Load].[DAS_LoadedEvents] ([EventFeed], [LastProcessedEventId]) 
	VALUES('AccountEvents', 0) 
END 