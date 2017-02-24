CREATE TABLE [Data_Load].[Das_Commitments]
(	
	[Id] BIGINT IDENTITY(1, 1), 
    [CommitmentID] BIGINT, 
    [PaymentStatusID] INT,            
    [ApprenticeshipID]  BIGINT,
    [AgreementStatusID] INT,
    [UKPRN]             BIGINT, 
    [ULN]               BIGINT,  
    [EmployerAccountID] VARCHAR(255),
    [TrainingTypeID]    INT,             
    [TrainingID]        VARCHAR(255),
    [TrainingStartDate] DATE,
    [TrainingEndDate]   DATE,
    [TrainingTotalCost] DECIMAL,
    [UpdateDateTime]    DATETIME
)
