CREATE PROCEDURE [Data_Load].[GetEventFailureCount]
	@eventId BIGINT
AS
	SELECT FailureCount FROM [Data_Load].[DAS_FailedEvents] WHERE EventId = @eventId