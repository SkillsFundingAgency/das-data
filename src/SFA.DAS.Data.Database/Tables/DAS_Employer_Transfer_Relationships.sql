CREATE TABLE [Data_Load].[DAS_Employer_Transfer_Relationships]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1), 
	[SenderAccountId] BIGINT NOT NULL,
    [ReceiverAccountId] BIGINT NOT NULL, 
    [RelationshipStatus] TINYINT NOT NULL, 
    [SenderUserId] BIGINT NOT NULL, 
    [ApproverUserId] BIGINT NULL, 
    [RejectorUserId] BIGINT NULL, 
    [UpdateDateTime] DATETIME NOT NULL DEFAULT GETDATE(), 
    [IsLatest] BIT NOT NULL DEFAULT 0  
)
GO

CREATE INDEX [IX_DAS_Employer_Transfer_Relationships_Sender_Receiver] ON [Data_Load].[DAS_Employer_Transfer_Relationships] (SenderAccountId, ReceiverAccountId) INCLUDE (IsLatest)
GO

CREATE INDEX [IX_DAS_Employer_Transfer_Relationships_IsLatest] ON [Data_Load].[DAS_Employer_Transfer_Relationships] ([IsLatest])
GO