CREATE TABLE [Data_Load].[Das_Commitments]
(	
	[Id] BIGINT IDENTITY(1, 1), 
    [CommitmentID] BIGINT, 
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
    [UpdateDateTime]    DATETIME
)
