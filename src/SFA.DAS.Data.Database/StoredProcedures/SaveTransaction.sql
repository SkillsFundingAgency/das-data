CREATE PROCEDURE [Data_Load].[SaveTransaction]
	@DasAccountId NVARCHAR(100),
    @DateCreated DATETIME,
    @SubmissionId BIGINT = NULL,
    @TransactionDate DATETIME,
    @TransactionType NVARCHAR(100),
    @LevyDeclared DECIMAL = NULL,
    @Amount DECIMAL,
    @PayeSchemeRef NVARCHAR(20) = NULL,
    @PeriodEnd NVARCHAR(50) = NULL,
    @UkPrn BIGINT = NULL
AS
	INSERT INTO [Data_Load].DAS_Transactions
	(
		[DasAccountId],
		[DateCreated],
		[SubmissionId],
		[TransactionDate],
		[TransactionType],
		[LevyDeclared],
		[Amount],
		[PayeSchemeRef],
		[PeriodEnd],
		[UkPrn]
	)
	VALUES
	(
		@DasAccountId,
		@DateCreated,
		@SubmissionId,
		@TransactionDate,
		@TransactionType,
		@LevyDeclared,
		@Amount,
		@PayeSchemeRef,
		@PeriodEnd,
		@UkPrn
	)