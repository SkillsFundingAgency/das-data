CREATE PROCEDURE [Data_Load].[GetEventFailureCount]
	@eventId NVARCHAR(100)
AS
	SELECT FailureCount FROM [Data_Load].[DAS_FailedEvents] WHERE EventId = @eventId