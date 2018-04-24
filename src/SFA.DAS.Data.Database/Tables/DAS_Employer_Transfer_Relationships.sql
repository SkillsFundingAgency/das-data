CREATE TABLE [Data_Load].[DAS_Employer_Transfer_Relationships]
(
	[Sender Account Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [Receiver Account Id] BIGINT NOT NULL, 
    [Relationship Status] NVARCHAR(10) NOT NULL, 
    [Sender User Id] BIGINT NOT NULL, 
    [Approver User Id] BIGINT NOT NULL, 
    [Rejector User Id] BIGINT NOT NULL DEFAULT (GETDATE()) 
)
GO
