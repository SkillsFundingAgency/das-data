CREATE PROCEDURE [Data_Load].[SaveDataLock_Period]
	@DataLockId BIGINT,
	@ApprenticeshipVersion VARCHAR(25),
	@CollectionPeriodName VARCHAR(8),
	@CollectionPeriodMonth INT,
	@CollectionPeriodYear INT,
	@IsPayable BIT,
	@TransactionType INT
AS
BEGIN
	SET NOCOUNT ON;
	
	BEGIN

		INSERT INTO [Data_Load].[DAS_DataLock_Periods]
		(
			[DataLockId],
			[ApprenticeshipVersion],
			[CollectionPeriodName],
			[CollectionPeriodMonth],
			[CollectionPeriodYear],
			[IsPayable],
			[TransactionType]
		)
		VALUES
		(
			@DataLockId,
			@ApprenticeshipVersion,
			@CollectionPeriodName,
			@CollectionPeriodMonth,
			@CollectionPeriodYear,
			@IsPayable,
			@TransactionType
		)

	END
END
GO
