CREATE VIEW [Data_Pub].[DAS_Employer_Transfer_Relationship]	AS
	SELECT
		[Id], 
		[SenderAccountId],
		[ReceiverAccountId], 
		[RelationshipStatus], 
		[SenderUserId], 
		[ApproverUserId], 
		[RejectorUserId],
		[UpdateDateTime]
	FROM 
		[Data_Load].[DAS_Employer_Transfer_Relationships]
	WHERE
		[IsLatest] = 1
