CREATE PROCEDURE [Data_Load].[CreateCommitmentApprenticeship]
	@commitmentId bigint,
	@paymentStatusId int,
	@apprenticeshipId bigint,
	@agreementStatusId int,
	@ukPrn varchar(255),
	@uln varchar(255),
	@employerAccountId varchar(255),
	@trainingTypeId varchar(255),
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
		GETUTCDATE()
	)