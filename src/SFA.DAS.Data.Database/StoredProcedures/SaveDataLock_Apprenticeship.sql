CREATE PROCEDURE [Data_Load].[SaveDataLock_Apprenticeship]
	@DataLockId BIGINT,
	@Version VARCHAR(25) = NULL,
	@StartDate DATE = NULL,
	@StandardCode BIGINT = NULL,
	@ProgrammeType INT = NULL,
	@FrameworkCode INT = NULL,
	@PathwayCode	 INT = NULL,
	@NegotiatedPrice DECIMAL(15, 2) = NULL,
	@EffectiveDate DATE = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	BEGIN

		INSERT INTO [Data_Load].[DAS_DataLock_Apprenticeships]
		(
			[DataLockId],
			[Version],
			[StartDate],
			[StandardCode],
			[ProgrammeType],
			[FrameworkCode],
			[PathwayCode],
			[NegotiatedPrice],
			[EffectiveDate]
		)
		VALUES
		(
			@DataLockId,
			@Version,
			@StartDate,
			@StandardCode,
			@ProgrammeType,
			@FrameworkCode,
			@PathwayCode,
			@NegotiatedPrice,
			@EffectiveDate
		)

	END
END
GO
