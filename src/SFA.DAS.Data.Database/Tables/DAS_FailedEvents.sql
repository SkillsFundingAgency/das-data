CREATE TABLE [Data_Load].[DAS_FailedEvents]
(
	[EventId] NVARCHAR(100) NOT NULL PRIMARY KEY,
	[FailureCount] INT NOT NULL,
	[LastFailureDate] DATETIME NULL
)
