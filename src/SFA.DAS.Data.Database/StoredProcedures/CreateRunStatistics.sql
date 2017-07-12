CREATE PROCEDURE [PerformancePlatform].[CreateRunStatistics]
	@dataType NVARCHAR(255),
	@runDateTime DATETIME,
	@numberOfRecords BIGINT
AS
	INSERT INTO [PerformancePlatform].[PP_HistoricalStatistics]
		(RunDateTime, DataType, NumberOfRecords)
	VALUES
		(@runDateTime, @dataType, @numberOfRecords)
