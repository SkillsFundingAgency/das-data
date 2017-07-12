CREATE TABLE [PerformancePlatform].[PP_HistoricalStatistics]
(
	[Id] BIGINT NOT NULL IDENTITY PRIMARY KEY, 
    [RunDateTime] DATETIME NOT NULL, 
    [DataType] NVARCHAR(255) NOT NULL, 
    [NumberOfRecords] BIGINT NOT NULL
)
