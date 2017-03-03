CREATE TABLE [Data_Load].[Das_Commitments]
(	
	[Id] BIGINT IDENTITY(1, 1), 
    [CommitmentID] BIGINT, 
    [PaymentStatusID] INT,            
    [ApprenticeshipID]  BIGINT,
    [AgreementStatusID] INT,
    [UKPRN]             VARCHAR(255), 
    [ULN]               VARCHAR(255),  
    [EmployerAccountID] VARCHAR(255),
    [TrainingTypeID]    VARCHAR(255),             
    [TrainingID]        VARCHAR(255),
    [TrainingStartDate] DATE,
    [TrainingEndDate]   DATE,
    [TrainingTotalCost] DECIMAL,
    [UpdateDateTime]    DATETIME
)
