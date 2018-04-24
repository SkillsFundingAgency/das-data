CREATE TABLE [Data_Load].[DAS_Employer_Transfer_Relationships]
(
	Id BIGINT NOT NULL PRIMARY KEY IDENTITY, 
	[SenderAccountId] BIGINT NOT NULL,
    [ReceiverAccountId] BIGINT NOT NULL, 
    [RelationshipStatus] NVARCHAR(50) NOT NULL, 
    [SenderUserId] BIGINT NULL, 
    [ApproverUserId] BIGINT NULL, 
    [RejectorUserId] BIGINT NULL, 
    [UpdatedDateTime] DATETIME NULL DEFAULT (GetDate()), 
    [IsLatest] BIT NULL DEFAULT 0  
)
GO
