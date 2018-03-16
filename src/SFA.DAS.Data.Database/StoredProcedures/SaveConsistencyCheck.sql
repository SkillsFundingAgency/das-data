CREATE PROCEDURE [Data_Load].[SaveConsistencyCheck]
	@dataType VARCHAR(50),
	@checkedDateTime DATETIME,
	@sourceSystemCount BIGINT,
	@rdsCount BIGINT
AS
SET NOCOUNT ON;

	INSERT INTO [Data_Load].[DAS_ConsistencyCheck]
           ([DataType]
           ,[CheckedDateTime]
           ,[SourceSystemCount]
           ,[RdsCount])
     VALUES
           (@dataType
           ,@checkedDateTime
           ,@sourceSystemCount
           ,@rdsCount)
GO