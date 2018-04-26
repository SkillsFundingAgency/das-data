CREATE PROCEDURE [Data_Load].[SaveTransfers]
	@transfers [Data_Load].[TransferEntity] readonly
AS
BEGIN
	INSERT INTO [Data_Load].[DAS_Employer_Account_Transfers]
	(
		[SenderAccountId],
		[ReceiverAccountId],
		[RequiredPaymentId],
		[CommitmentId],
		[Amount],
		[Type],
		[CollectionPeriodName]
	)
	SELECT
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
			WHERE t.[SenderAccountId] = t2.[SenderAccountId]
				AND t.[ReceiverAccountId] = t2.[ReceiverAccountId]
				AND t.[RequiredPaymentId] = t2.[RequiredPaymentId]
				AND t.[CommitmentId] = t2.[CommitmentId]
				AND t.[Amount] = t2.[Amount]
				AND t.[Type] = t2.[Type]
				AND t.[CollectionPeriodName] = t2.[CollectionPeriodName]
		)
END