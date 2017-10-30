CREATE PROCEDURE [Data_Load].[SavePayment]
	@PaymentId NVARCHAR(100),
	@UkPrn BIGINT,
	@Uln BIGINT,
	@EmployerAccountId NVARCHAR(100) = NULL,
	@ApprenticeshipId BIGINT = NULL,
	@DeliveryMonth INT,
	@DeliveryYear INT,
	@CollectionMonth INT,
	@CollectionYear INT,
	@EvidenceSubmittedOn DATETIME,
	@EmployerAccountVersion NVARCHAR(50),
	@ApprenticeshipVersion NVARCHAR(50),
	@FundingSource NVARCHAR(25),
	@TransactionType NVARCHAR(50),
	@Amount DECIMAL(18,5),
	@StandardCode BIGINT = NULL,
	@FrameworkCode INT = NULL,
	@ProgrammeType INT = NULL,
	@PathwayCode INT = NULL,
	@ContractType NVARCHAR(50)
AS

	IF NOT EXISTS(SELECT TOP 1 1 FROM [Data_Load].DAS_Payments WHERE PaymentId = @PaymentId)
	BEGIN

		INSERT INTO [Data_Load].DAS_Payments 
		(
			[PaymentId],
			[UkPrn],
			[Uln],
			[EmployerAccountId],
			[ApprenticeshipId],
			[DeliveryMonth],
			[DeliveryYear],
			[CollectionMonth],
			[CollectionYear],
			[EvidenceSubmittedOn],
			[EmployerAccountVersion],
			[ApprenticeshipVersion],
			[FundingSource],
			[TransactionType],
			[Amount],
			[StandardCode],
			[FrameworkCode],
			[ProgrammeType],
			[PathwayCode],
			[ContractType] 
		)
		VALUES
		(
			@PaymentId,
			@UkPrn,
			@Uln,
			@EmployerAccountId,
			@ApprenticeshipId,
			@DeliveryMonth,
			@DeliveryYear,
			@CollectionMonth,
			@CollectionYear,
			@EvidenceSubmittedOn,
			@EmployerAccountVersion,
			@ApprenticeshipVersion,
			@FundingSource,
			@TransactionType,
			@Amount,
			@StandardCode,
			@FrameworkCode,
			@ProgrammeType,
			@PathwayCode,
			@ContractType
		)

	END