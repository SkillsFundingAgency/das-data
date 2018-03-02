CREATE TABLE [Data_Load].[DAS_ConsistencyCheck]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [DataType] VARCHAR(50) NOT NULL, 
    [CheckedDateTime] DATETIME NOT NULL, 
    [SourceSystemCount] INT NOT NULL, 
    [RdsCount] INT NOT NULL
)
GO
CREATE INDEX [IX_ConsistencyCheck_DataType] ON [Data_Load].[DAS_ConsistencyCheck] ([DataType])
GO