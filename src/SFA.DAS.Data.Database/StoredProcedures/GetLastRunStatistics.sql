CREATE PROCEDURE [PerformancePlatform].[GetLastRunStatistics]
	@dataType NVARCHAR(255)
AS
	SELECT TOP 1 NumberOfRecords FROM [PerformancePlatform].[PP_HistoricalStatistics]
	WHERE DataType = @dataType
	ORDER BY RunDateTime DESC
