CREATE TABLE [Data_Load].[DAS_Employer_Account_Transfers]
(
	[SenderAccountId] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [ReceiverAccountId] BIGINT NOT NULL, 
    [RelationshipStatus] NVARCHAR(10) NOT NULL, 
    [SenderUserId] BIGINT NOT NULL, 
    [ApproverUserId] BIGINT NOT NULL, 
    [RejectorUserId] BIGINT NOT NULL , 
    [Required Payment ID] UNIQUEIDENTIFIER NOT NULL, 
    [ColectionPeriodName] NCHAR(10) NOT NULL
)
GO