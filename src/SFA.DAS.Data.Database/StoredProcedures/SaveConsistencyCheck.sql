CREATE PROCEDURE [Data_Load].[SaveConsistencyCheck]
	@dataType VARCHAR(50),
	@checkedDateTime DATETIME,
	@sourceSystemCount BIGINT,
	@rdsCount BIGINT
AS
SET NOCOUNT ON;
SET XACT_ABORT ON;
SET TRANSACTION ISOLATION LEVEL REPEATABLE READ;

BEGIN TRANSACTION;

	EXEC sp_getapplock @Resource='ConsistencyCheckLock', @LockMode='Exclusive';
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
COMMIT;
GO