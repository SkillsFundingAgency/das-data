CREATE TABLE [Data_Load].[Das_Commitments]
(	
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [CommitmentID] BIGINT NOT NULL, 
    [PaymentStatus] VARCHAR(50),            
    [ApprenticeshipID]  BIGINT,
    [AgreementStatus] VARCHAR(50),
    [ProviderID]             VARCHAR(255), 
    [LearnerID]               VARCHAR(255),  
    [EmployerAccountID] VARCHAR(255),
    [TrainingTypeID]    VARCHAR(255),             
    [TrainingID]        VARCHAR(255),
    [TrainingStartDate] DATE,
    [TrainingEndDate]   DATE,
    [TrainingTotalCost] DECIMAL,
    [UpdateDateTime]    DATETIME NOT NULL, 
    [LegalEntityCode] NVARCHAR(50) NULL, 
    [LegalEntityName] NVARCHAR(100) NULL, 
    [LegalEntityOrganisationType] NVARCHAR(20) NULL,
	[DateOfBirth] DATETIME NULL, 
    [IsLatest] BIT NOT NULL DEFAULT 0, 
    [TransferSenderAccountId] BIGINT NULL, 
    [TransferApprovalStatus] NVARCHAR(50) NULL,
    [TransferApprovalDate] DATETIME NULL
)
GO
CREATE INDEX [IX_Commitment_Apprenticeship] ON [Data_Load].[Das_Commitments] ([ApprenticeshipID], [IsLatest])
GO
CREATE INDEX [IX_Commitment_IsLatest_AgreementStatus_StartDate] ON [Data_Load].[Das_Commitments] ([IsLatest], [AgreementStatus], [TrainingStartDate])
GO
CREATE INDEX [IX_Commitment_IsLatest_PaymentStatus] ON [Data_Load].[Das_Commitments] ([IsLatest], [PaymentStatus])
GO
CREATE INDEX [IX_Commitment_EmployerAccount_IsLatest] ON [Data_Load].[Das_Commitments] ([EmployerAccountID], [IsLatest])