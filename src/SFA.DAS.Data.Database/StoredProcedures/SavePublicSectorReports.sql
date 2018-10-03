CREATE PROCEDURE [Data_Load].[SavePublicSectorReports]
	@dasAccountId nvarchar(6),
	@organisationName NVARCHAR(100),
	@reportingPeriod int,
	@figureA int,
	@figureB int,
	@figureE decimal(10,4),
	@figureC INT,
	@figureD INT,
	@figureF DECIMAL(10,4),
	@figureG INT,
	@figureH INT,
	@figureI DECIMAL(10,4),
	@fullTimeEquivalent INT,
	@outlineActions NVARCHAR(4000),
	@outlineActionsWordCount INT,
	@challenges NVARCHAR(4000),
	@challengesWordCount INT,
	@targetPlans NVARCHAR(4000),
	@targetPlansWordCount INT,
	@anythingElse NVARCHAR(4000),
	@anythingElseWordCount INT,
	@submittedAt DATETIME,
	@submittedName NVARCHAR(250),
	@submittedEmail NVARCHAR(250)
AS

	SET NOCOUNT ON;
	SET ANSI_NULLS OFF

	UPDATE [Data_Load].[DAS_PublicSector_Reports]
	SET IsLatest = 0
	WHERE [DasAccountId] = @dasAccountId 
		AND [ReportingPeriod] = @ReportingPeriod
		AND IsLatest = 1

	INSERT INTO [Data_Load].[DAS_PublicSector_Reports]
	(
		[DasAccountId],
		[OrganisationName],
		[ReportingPeriod],
		[FigureA],
		[FigureB],
		[FIgureE],
		[FigureC],
		[FigureD],
		[FigureF],
		[FigureG],
		[FigureH],
		[FigureI],
		[FullTimeEquivalent],
		[OutlineActions],
		[OutlineActionsWordCount],
		[Challenges],
		[ChallengesWordCount],
		[TargetPlans],
		[TargetPlansWordCount],
		[AnythingElse],
		[AnythingElseWordCount],
		[SubmittedAt],
		[SubmittedName],
		[SubmittedEmail],
		[IsLatest]
	)
	VALUES
	(
		@dasAccountId,
		@organisationName,
		@reportingPeriod,
		@figureA,
		@figureB,
		@figureE,
		@figureC,
		@figureD,
		@figureF,
		@figureG,
		@figureH,
		@figureI,
		@fullTimeEquivalent,
		@outlineActions,
		@outlineActionsWordCount,
		@challenges,
		@challengesWordCount,
		@targetPlans,
		@targetPlansWordCount,
		@anythingElse,
		@anythingElseWordCount,
		@submittedAt,
		@submittedName,
		@submittedEmail,
		1
	)
