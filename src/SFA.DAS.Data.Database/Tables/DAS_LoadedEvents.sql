CREATE TABLE [Data_Load].[DAS_LoadedEvents]
(
	[EventFeed] NVARCHAR(50) NOT NULL PRIMARY KEY, 
    [LastProcessedEventId] NVARCHAR(100) NOT NULL
)
