CREATE PROCEDURE [Data_Load].[SetEventFailureCount]
	@eventId BIGINT,
	@failureCount INT
AS
	MERGE [Data_Load].[DAS_FailedEvents] AS [T] USING (SELECT @eventId AS EventId) AS [S] ON [T].EventId = [S].EventId
    WHEN MATCHED THEN UPDATE SET [T].FailureCount = @failureCount WHEN NOT MATCHED THEN INSERT(EventId, FailureCount) VALUES([S].EventId, @failureCount);