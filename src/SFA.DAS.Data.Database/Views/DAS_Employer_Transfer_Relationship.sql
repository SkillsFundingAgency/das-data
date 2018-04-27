CREATE VIEW [Data_Pub].[DAS_Employer_Transfer_Relationship]	AS
	SELECT
		T.[Id], 
		[SenderAccountId],
		[ReceiverAccountId], 
		[RelationshipStatus], 
		[SenderUserId], 
		[ApproverUserId], 
		[RejectorUserId],
		T.[UpdateDateTime],
		T.[IsLatest],
		senderAcc.DasAccountId as [SenderDasAccountID],
		recieverAcc.DasAccountId as [RecieverDasAccountID]
	FROM 
		[Data_Load].[DAS_Employer_Transfer_Relationships] as T
		left join [Data_Load].[DAS_Employer_Accounts] as senderAcc on senderAcc.AccountId = T.SenderAccountId
		left join [Data_Load].[DAS_Employer_Accounts] as recieverAcc on recieverAcc.AccountId = T.ReceiverAccountId