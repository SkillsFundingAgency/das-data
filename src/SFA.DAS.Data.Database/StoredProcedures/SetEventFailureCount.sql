CREATE PROCEDURE [Data_Load].[SetEventFailureCount]
	@eventId NVARCHAR(100),
	@failureCount INT
AS
	MERGE [Data_Load].[DAS_FailedEvents] AS [T] USING (SELECT @eventId AS EventId) AS [S] ON [T].EventId = [S].EventId
    WHEN MATCHED THEN UPDATE SET [T].FailureCount = @failureCount, [T].[LastFailureDate] = GETDATE() WHEN NOT MATCHED THEN INSERT(EventId, FailureCount, LastFailureDate) VALUES([S].EventId, @failureCount, GETDATE());