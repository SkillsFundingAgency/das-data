CREATE PROCEDURE [Data_Load].[SaveSubmissionsSummary]
	@submittedTotals int,
	@inProcessTotals  int,
	@viewedTotals int,
	@reportingPeriod int
AS
	SET NOCOUNT ON;
	SET ANSI_NULLS OFF

	UPDATE [Data_Load].[DAS_PublicSector_Summary] SET IsLatest = 0 WHERE IsLatest = 1

	INSERT INTO [Data_Load].[DAS_PublicSector_Summary]
	(
		[SubmittedTotals],
		[InProcessTotals],
		[ViewedTotals],
		[ReportingPeriod],
		[IsLatest]
	)
	VALUES
	(
		@submittedTotals,
		@inProcessTotals,
		@viewedTotals,
		@reportingPeriod,
		1
	)