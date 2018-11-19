CREATE PROCEDURE [Data_Load].[SaveDataLock_Error]
	@DataLockId BIGINT,
	@ErrorCode VARCHAR(15),
	@SystemDescription	NVARCHAR(255) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	BEGIN

		INSERT INTO [Data_Load].[DAS_DataLock_Errors]
		(
			[DataLockId],
			[ErrorCode],
			[SystemDescription]
		)
		VALUES
		(
			@DataLockId,
			@ErrorCode,
			@SystemDescription
		)

	END
END
GO
