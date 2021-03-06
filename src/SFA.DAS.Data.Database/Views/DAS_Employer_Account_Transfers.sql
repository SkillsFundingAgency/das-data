﻿CREATE VIEW [Data_Pub].[DAS_Employer_Account_Transfers]	AS 
	SELECT
		[TransferId],
		[SenderAccountId], 
		[ReceiverAccountId], 
		[RequiredPaymentId], 
		[CommitmentId],
		[Amount],
		[Type],
		[CollectionPeriodName],
		[UpdateDateTime]
	FROM 
		[Data_Load].[DAS_Employer_Account_Transfers]
