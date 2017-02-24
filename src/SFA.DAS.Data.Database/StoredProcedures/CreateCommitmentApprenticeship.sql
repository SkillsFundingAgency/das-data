CREATE PROCEDURE [Data_Load].[CreateCommitmentApprenticeship]
	@commitmentId bigint,
	@paymentStatusId int,
	@apprenticeshipId bigint,
	@agreementStatusId int,
	@ukPrn bigint,
	@uln bigint,
	@employerAccountId varchar(255),
	@trainingTypeId int,
	@trainingId varchar(255),
	@trainingStartDate Date,
	@trainingEndDate Date,
	@trainingTotalCost decimal
AS
	INSERT INTO [Data_Load].Das_Commitments 
	(
		CommitmentID, 
		PaymentStatusID, 
		ApprenticeshipID, 
		AgreementStatusID, 
		UKPRN, 
		ULN,
		EmployerAccountID, 
		TrainingTypeID,
		TrainingID,
		TrainingStartDate,
		TrainingEndDate,
		TrainingTotalCost,
		UpdateDateTime
	)
	VALUES
	(
		@commitmentId,
		@paymentStatusId,
		@apprenticeshipId,
		@agreementStatusId,
		@ukPrn,
		@uln,
		@employerAccountId,
		@trainingTypeId,
		@trainingId,
		@trainingStartDate,
		@trainingEndDate,
		@trainingTotalCost,
		GETDATE()
	)