CREATE TABLE [Data_Load].[DAS_Employer_Transfer_Relationships]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1), 
	[SenderAccountId] BIGINT NOT NULL,
    [ReceiverAccountId] BIGINT NOT NULL, 
    [RelationshipStatus] TINYINT NOT NULL, 
    [SenderUserId] BIGINT NOT NULL, 
    [ApproverUserId] BIGINT NULL, 
    [RejectorUserId] BIGINT NULL  
)
GO
