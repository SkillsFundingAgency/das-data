CREATE PROCEDURE [Data_Load].[SaveTransfers]
	@transfers [Data_Load].[TransferEntity] readonly
AS
BEGIN
	INSERT INTO [Data_Load].[DAS_Employer_Account_Transfers]
	(
		[TransferId],
		[SenderAccountId],
		[ReceiverAccountId],
		[RequiredPaymentId],
		[CommitmentId],
		[Amount],
		[Type],
		[CollectionPeriodName]
	)
	SELECT
		[TransferId],
		[SenderAccountId],
		[ReceiverAccountId],
		[RequiredPaymentId],
		[CommitmentId],
		[Amount],
		[Type],
		[CollectionPeriodName]
	FROM
		@transfers t
	WHERE
		NOT EXISTS
		(
			SELECT 1 
			FROM [Data_Load].[DAS_Employer_Account_Transfers] t2 
			WHERE t.[TransferId] = t2.[TransferId]
		)
END