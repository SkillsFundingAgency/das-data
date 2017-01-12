CREATE TABLE [Data_Load].[DAS_LoadedEvents]
(
	[EventFeed] NVARCHAR(50) NOT NULL PRIMARY KEY, 
    [LastProcessedEventId] BIGINT NOT NULL
)
