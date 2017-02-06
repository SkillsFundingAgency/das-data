CREATE PROCEDURE [Data_Load].[StoreLastProcessedEventId]
	@eventFeed NVARCHAR(50),
	@lastProcessedEventId BIGINT
AS
	UPDATE [Data_Load].[DAS_LoadedEvents] SET LastProcessedEventId = @lastProcessedEventId
    WHERE EventFeed = @eventFeed
