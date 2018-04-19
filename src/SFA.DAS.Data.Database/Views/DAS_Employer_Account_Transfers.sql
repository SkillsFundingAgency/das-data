CREATE VIEW [Data_Pub].[DAS_Employer_Account_Transfers]	AS 
	SELECT
		[Id],
		[SenderAccountId], 
		[ReceiverAccountId], 
		[RequiredPaymentId], 
		[CommitmentId],
		[Amount],
		[Type],
		[TransferDate],
		[ColectionPeriodName]
	FROM 
		[Data_Load].[DAS_Employer_Account_Transfers]
