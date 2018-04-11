CREATE TABLE [Data_Load].[DAS_Employer_Transfer_Relationships]
(
	Id BIGINT NOT NULL PRIMARY KEY IDENTITY, 
	[SenderAccountId] BIGINT NOT NULL,
    [ReceiverAccountId] BIGINT NOT NULL, 
    [RelationshipStatus] INT NOT NULL, 
    [SenderUserId] BIGINT NULL, 
    [ApproverUserId] BIGINT NULL, 
    [RejectorUserId] BIGINT NULL  
)
GO
