CREATE PROCEDURE [PerformancePlatform].[SetPerformancePlatformLastRun]
	@runDateTime DATETIME
AS
	UPDATE [PerformancePlatform].[PP_LastRun] SET [DateTime] = @runDateTime