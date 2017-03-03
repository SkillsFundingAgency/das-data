CREATE PROCEDURE [Data_Load].[StoreLastProcessedEventId]
	@eventFeed NVARCHAR(50),
	@lastProcessedEventId BIGINT
AS
	MERGE [Data_Load].[DAS_LoadedEvents] AS [Target]
	USING (SELECT @eventFeed AS Feed) AS [Source] 
	ON [Target].EventFeed = [Source].Feed
	WHEN MATCHED THEN UPDATE SET [Target].LastProcessedEventId =  @lastProcessedEventId
	WHEN NOT MATCHED THEN  INSERT (EventFeed, LastProcessedEventId) VALUES (@eventFeed, @lastProcessedEventId);