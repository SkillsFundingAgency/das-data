CREATE TABLE [Data_Load].[PP_HistoricalStatistics]
(
	[Id] BIGINT NOT NULL PRIMARY KEY, 
    [RunDateTime] DATETIME NOT NULL, 
    [DataType] NVARCHAR(255) NOT NULL, 
    [NumberOfRecords] BIGINT NOT NULL
)
