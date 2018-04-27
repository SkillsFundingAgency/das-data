CREATE VIEW [Data_Pub].[DAS_Employer_Transfer_Relationship]	AS
	SELECT
		[Id], 
		[SenderAccountId],
		[ReceiverAccountId], 
		[RelationshipStatus], 
		[SenderUserId], 
		[ApproverUserId], 
		[RejectorUserId]
	FROM 
		[Data_Load].[DAS_Employer_Transfer_Relationships]
	WHERE
		[Data_Load].[DAS_Employer_Transfer_Relationships].[IsLatest] = 1
