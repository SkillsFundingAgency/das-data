CREATE PROCEDURE [Data_Load].[CreateCommitmentApprenticeship]
	@commitmentId bigint,
	@paymentStatus varchar(50),
	@apprenticeshipId bigint,
	@agreementStatus varchar(50),
	@ukPrn varchar(255),
	@uln varchar(255),
	@employerAccountId varchar(255),
	@trainingTypeId varchar(255),
	@trainingId varchar(255),
	@trainingStartDate Date,
	@trainingEndDate Date,
	@trainingTotalCost decimal,
	@legalEntityCode NVARCHAR(50), 
    @legalEntityName NVARCHAR(100), 
    @legalEntityOrganisationType NVARCHAR(20)
AS
	INSERT INTO [Data_Load].Das_Commitments 
	(
		CommitmentID, 
		PaymentStatus, 
		ApprenticeshipID, 
		AgreementStatus, 
		ProviderID, 
		LearnerID,
		EmployerAccountID, 
		TrainingTypeID,
		TrainingID,
		TrainingStartDate,
		TrainingEndDate,
		TrainingTotalCost,
		LegalEntityCode,
		LegalEntityName,
		LegalEntityOrganisationType,
		UpdateDateTime
	)
	VALUES
	(
		@commitmentId,
		@paymentStatus,
		@apprenticeshipId,
		@agreementStatus,
		@ukPrn,
		@uln,
		@employerAccountId,
		@trainingTypeId,
		@trainingId,
		@trainingStartDate,
		@trainingEndDate,
		@trainingTotalCost,
		@legalEntityCode,
		@legalEntityName,
		@legalEntityOrganisationType,
		GETUTCDATE()
	)