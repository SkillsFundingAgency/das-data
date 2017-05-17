﻿CREATE PROCEDURE [Data_Load].[SaveLevyDeclaration]
	@DasAccountId NVARCHAR(100),
    @LevyDeclarationId BIGINT,
    @PayeSchemeReference NVARCHAR(20),
	@LevyDueYearToDate DECIMAL = NULL,
    @LevyAllowanceForYear DECIMAL = NULL,
	@SubmissionDate DATETIME = NULL,
    @SubmissionId BIGINT,
    @PayrollYear NVARCHAR(10),
    @PayrollMonth TINYINT,
    @CreatedDate DATETIME,
    @EndOfYearAdjustment BIT,
    @EndOfYearAdjustmentAmount DECIMAL = NULL,
    @DateCeased DATETIME = NULL,
	@InactiveFrom DATETIME = NULL,
    @InactiveTo DATETIME = NULL,
    @HmrcSubmissionId BIGINT,
    @EnglishFraction DECIMAL,
    @TopupPercentage DECIMAL,
	@TopupAmount DECIMAL,
	@LevyDeclaredInMonth DECIMAL = NULL,
	@LevyAvailableInMonth DECIMAL = NULL
AS
	INSERT INTO [Data_Load].DAS_LevyDeclarations
	(
		[DasAccountId],
		[LevyDeclarationId],
		[PayeSchemeReference],
		[LevyDueYearToDate],
		[LevyAllowanceForYear],
		[SubmissionDate],
		[SubmissionId],
		[PayrollYear],
		[PayrollMonth],
		[CreatedDate],
		[EndOfYearAdjustment],
		[EndOfYearAdjustmentAmount],
		[DateCeased],
		[InactiveFrom],
		[InactiveTo],
		[HmrcSubmissionId],
		[EnglishFraction],
		[TopupPercentage],
		[TopupAmount],
		[LevyDeclaredInMonth],
		[LevyAvailableInMonth]
	)
	VALUES
	(
		@DasAccountId,
		@LevyDeclarationId,
		@PayeSchemeReference,
		@LevyDueYearToDate,
		@LevyAllowanceForYear,
		@SubmissionDate,
		@SubmissionId,
		@PayrollYear,
		@PayrollMonth,
		@CreatedDate,
		@EndOfYearAdjustment,
		@EndOfYearAdjustmentAmount,
		@DateCeased,
		@InactiveFrom,
		@InactiveTo,
		@HmrcSubmissionId,
		@EnglishFraction,
		@TopupPercentage,
		@TopupAmount,
		@LevyDeclaredInMonth,
		@LevyAvailableInMonth
	)
