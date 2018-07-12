CREATE PROCEDURE [Data_Load].[SavePublicSectorReports]
	@accountId int,
	@reportingPeriod int,
	@figureA int,
	@figureB int,
	@figureE decimal,
	@figureC int,
	@figureD int,
	@figureF decimal,
	@figureG int,
	@figureH int,
	@figureI decimal,
	@outlineActions nvarchar(4000),
	@outlineActionsWordCount int,
	@challenges nvarchar(4000),
	@challengesWordCount int,
	@targetPlans nvarchar(4000),
	@targetPlansWordCount int,
	@anythingElse nvarchar(4000),
	@anythingElseWordCount int,
	@submittedAt datetime,
	@submittedName nvarchar(250),
	@submittedEmail nvarchar(250)
AS

	SET NOCOUNT ON;
	SET ANSI_NULLS OFF

	INSERT INTO [Data_Load].[DAS_PublicSector_Reports]
	(
		[AccountId],
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
		[SubmittedEmail]
	)
	VALUES
	(
		@accountId,
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
		@submittedEmail
		-- todo: ADD A LAST TIME RUN?
	)
