CREATE PROCEDURE [Data_Load].[SaveConsistencyCheck]
	@dataType VARCHAR(50),
	@checkedDateTime DATETIME,
	@sourceSystemCount BIGINT,
	@rdsCount BIGINT
AS
SET NOCOUNT ON;

UPDATE [Data_Load].[DAS_ConsistencyCheck] SET IsLatest = 0 WHERE IsLatest = 1 AND DataType = @dataType

	INSERT INTO [Data_Load].[DAS_ConsistencyCheck]
           ([DataType]
           ,[CheckedDateTime]
           ,[SourceSystemCount]
           ,[RdsCount]
		   ,[IsLatest])
     VALUES
           (@dataType
           ,@checkedDateTime
           ,@sourceSystemCount
           ,@rdsCount,
		   1)
GO