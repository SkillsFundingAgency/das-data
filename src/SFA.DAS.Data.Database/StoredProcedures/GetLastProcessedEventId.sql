CREATE PROCEDURE [Data_Load].[GetLastProcessedEventId]
	@eventFeed NVARCHAR(50)
AS
	SELECT LastProcessedEventId FROM [Data_Load].[DAS_LoadedEvents] WHERE EventFeed = @eventFeed